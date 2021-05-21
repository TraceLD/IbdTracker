using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.Entities;
using IbdTracker.Features.Doctors;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors
{
    public class VerifyTests : TestBase
    {
        public VerifyTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldVerifyAnExistingUnverifiedDoctor()
        {
            // arrange;
            var newDoctor = new Doctor
            {
                IsApproved = false,
                Location = "TEST",
                Name = "TEST",
                DoctorId = "TEST",
                OfficeHours = new()
            };
            
            await Context.Doctors.AddAsync(newDoctor);
            await Context.SaveChangesAsync();

            // act;
            var res = await SendMediatorRequestInScope(new Verify.Command(newDoctor.DoctorId));

            // assert;
            res.Should().BeOfType<NoContentResult>();

            // clean up;
            Context.Doctors.Remove(newDoctor);
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldReturnBadResultIfDoctorAlreadyVerified()
        {
            // arrange;
            var command = new Verify.Command(TestUserHelper.TestDoctorId);
            
            // act;
            var res = await SendMediatorRequestInScope(command);

            // assert;
            res.Should().BeOfType<BadRequestResult>();
        }
        
        [Fact]
        public async Task ShouldReturnNotFoundIfUnverifiedDoctorDoesNotExist()
        {
            // arrange;
            var command = new Verify.Command("LITERALLY_MADE_UP_ID");
            
            // act;
            var res = await SendMediatorRequestInScope(command);

            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }
    }
}