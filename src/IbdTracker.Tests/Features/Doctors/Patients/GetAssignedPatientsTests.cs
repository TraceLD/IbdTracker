using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Doctors.Patients;
using MediatR;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors.Patients
{
    public class GetAssignedPatientsTests : TestBase
    {
        public GetAssignedPatientsTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ReturnsAssignedPatientsWhenTheyExist()
        {
            // arrange;
            const string idOfPatientAssignedToTheTestDoctor = "auth0|unittests2";
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new GetAssignedPatients.Query());
            
            // assert;
            res.Should().Contain(x => x.PatientId.Equals(idOfPatientAssignedToTheTestDoctor));
        }

        [Fact]
        public async Task ReturnsEmptyListIfNoPatientsAssigned()
        {
            // arrange;
            // this doctor has no patients;
            const string doctorId = "auth0|60987c3569b62800686370ff";
            
            SetCurrentUser(doctorId);
            var mediator = GetService<IMediator>();

            // act;
            var res = await mediator.Send(new GetAssignedPatients.Query());

            // assert;
            res.Should().BeEmpty();
        }
    }
}