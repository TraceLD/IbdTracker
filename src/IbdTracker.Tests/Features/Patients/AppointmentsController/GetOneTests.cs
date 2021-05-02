using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Appointments;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.AppointmentsController
{
    public class GetOneTests : TestBase
    {
        public GetOneTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetExistingAppointment()
        {
            // arrange;
            // test data set contains this Appointment;
            var appointmentId = new Guid("134f3726-3369-492b-b3bd-b62be67b96f8");
            var expected = new AppointmentDto
            {
                AppointmentId = appointmentId,
                PatientId = TestUserHelper.TestPatientId,
                DoctorId = "auth0|6076cf2ec42780006af85a96",
                StartDateTime = new DateTime(2021, 7, 1, 10, 0, 0),
                PatientNotes = null,
                DurationMinutes = 60,
                DoctorNotes = null,
                DoctorName = "Dr Demo Person",
                Location = "10 Demo Street, Demo Town, DE1 MO0, United Kingdom"
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(appointmentId));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task ShouldReturnNullIfNotFound()
        {
            // arrange;
            // test data set contains this Appointment;
            // empty Guid, no appointment will have that, which is what we want, since we are testing
            // what happens if appointment isn't found;
            var appointmentId = new Guid();

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(appointmentId));
            
            // assert;
            res.Should().BeNull();
        }
    }
}