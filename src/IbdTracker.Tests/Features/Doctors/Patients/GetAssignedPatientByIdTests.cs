using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Core.Entities;
using IbdTracker.Features.Doctors.Patients;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors.Patients
{
    public class GetAssignedPatientByIdTests : TestBase
    {
        public GetAssignedPatientByIdTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldReturnPatientIfAssignedToTheDoctor()
        {
            // arrange;
            const string patientId = "auth0|unittests2";
            var expected = new PatientDto
            {
                PatientId = patientId,
                DoctorId = TestUserHelper.TestDoctorId,
                Name = "Integration and Unit Tests",
                DateDiagnosed = new(2021, 1, 1, 0, 0, 0),
                DateOfBirth = new(1990, 1, 1, 0, 0, 0),
                IbdType = IbdType.CrohnsDisease
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(
                new GetAssignedPatientById.Query(patientId));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnNullIfNotFound()
        {
            // arrange;
            const string patientId = "MADE_UP";

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(
                new GetAssignedPatientById.Query(patientId));
            
            // assert;
            res.Should().BeNull();
        }
    }
}