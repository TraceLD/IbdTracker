using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Doctors;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors
{
    public class GetCurrentTests : TestBase
    {
        public GetCurrentTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task GetsIfLoggedIn()
        {
            // arrange;
            const string doctorId = TestUserHelper.TestDoctorId;
            var expected = new DoctorDto
            {
                Name = "Dr Test Doctor",
                Location = "xUnit Land",
                DoctorId = doctorId,
                IsApproved = true
            };

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new GetCurrent.Query());
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldThrowIfNotLoggedIn()
        {
            // act;
            Func<Task<DoctorDto?>> res = async () => await SendMediatorRequestInScope(new GetCurrent.Query());
            
            // assert;
            res.Should().ThrowAsync<Exception>();
        }
    }
}