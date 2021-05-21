using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Doctors;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors
{
    public class GetByIdTests : TestBase
    {
        public GetByIdTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetByIdIfExists()
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
            var res = await SendMediatorRequestInScope(new GetById.Query(doctorId));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnNullIfUnapproved()
        {
            // arrange;
            // this doctor is unapproved in the test dataset;
            const string doctorId = "auth0|60987c3569b62800686370ff";

            // act;
            var res = await SendMediatorRequestInScope(new GetById.Query(doctorId));
            
            // assert;
            res.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnNullIfDoesNotExist()
        {
            // arrange;
            // this doctor does not exist in the test dataset;
            const string doctorId = "LITERALLY_MADE_UP";

            // act;
            var res = await SendMediatorRequestInScope(new GetById.Query(doctorId));
            
            // assert;
            res.Should().BeNull();
        }
    }
}