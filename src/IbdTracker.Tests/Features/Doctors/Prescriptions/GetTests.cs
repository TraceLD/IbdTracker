using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Doctors.Prescriptions;
using MediatR;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors.Prescriptions
{
    public class GetTests : TestBase
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ReturnsAssignedPrescriptionsIfTheyExist()
        {
            // arrange;
            var expected = new List<PrescriptionDto>
            {
                new()
                {
                    DoctorId = new("auth0|6099572d13cd7c0070164d6a"),
                    PatientId = "auth0|unittests2",
                    DoctorInstructions = "9mg every day after breakfast",
                    MedicationId = new("6a1f5c6a-25d8-4066-9de1-a0e2162b8af9"),
                    PrescriptionId = new("9bd7b589-93e5-418b-abfa-d8b4d3e82c70"),
                    EndDateTime = new(2021, 5, 31, 0, 0, 0),
                    StartDateTime = new(2021, 5, 1, 0, 0, 0),
                }
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query(null));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnEmptyIfNoPrescriptionsAssigned()
        {
            // arrange;
            // this doctor has no prescriptions;
            const string doctorId = "auth0|60987c3569b62800686370ff";
            
            SetCurrentUser(doctorId);
            var mediator = GetService<IMediator>();

            // act;
            var res = await mediator.Send(new Get.Query(null));

            // assert;
            res.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnsByPatientIdIfExists()
        {
            // arrange;
            const string patientId = "auth0|unittests2";
            var expected = new List<PrescriptionDto>
            {
                new()
                {
                    DoctorId = new("auth0|6099572d13cd7c0070164d6a"),
                    PatientId = patientId,
                    DoctorInstructions = "9mg every day after breakfast",
                    MedicationId = new("6a1f5c6a-25d8-4066-9de1-a0e2162b8af9"),
                    PrescriptionId = new("9bd7b589-93e5-418b-abfa-d8b4d3e82c70"),
                    EndDateTime = new(2021, 5, 31, 0, 0, 0),
                    StartDateTime = new(2021, 5, 1, 0, 0, 0),
                }
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query(patientId));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnsEmptyListIfNotFoundByPatientId()
        {
            // arrange;
            const string patientId = "MADE_UP";

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query(patientId));
            
            // assert;
            res.Should().BeEmpty();
        }
    }
}