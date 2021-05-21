using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Appointments;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.Appointments
{
    public class GetAllTests : TestBase
    {
        public GetAllTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetAppointments()
        {
            // arrange;
            // test data set contains the following 1 appointment:
            var expected = new List<AppointmentDto>
            {
                new()
                {
                    AppointmentId = new("134f3726-3369-492b-b3bd-b62be67b96f8"),
                    PatientId = TestUserHelper.TestPatientId,
                    PatientName = "Integration and Unit Tests",
                    DoctorId = "auth0|6076cf2ec42780006af85a96",
                    StartDateTime = new(2021, 7, 1, 10, 0, 0),
                    PatientNotes = null,
                    DurationMinutes = 60,
                    DoctorNotes = null,
                    DoctorName = "Dr Demo Person",
                    Location = "10 Demo Street, Demo Town, DE1 MO0, United Kingdom"
                }
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetAll.Query());
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }
    }
}