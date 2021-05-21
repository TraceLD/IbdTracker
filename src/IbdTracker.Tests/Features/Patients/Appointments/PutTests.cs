using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Patients.Appointments;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.Appointments
{
    public class PutTests : TestBase
    {
        public PutTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPutAValidAppointment()
        {
            // arrange;
            // appointment with this ID exists in the test data set so can be modified;
            var appointmentId = new Guid("134f3726-3369-492b-b3bd-b62be67b96f8");
            var command = new Put.Command(
                appointmentId,
                "auth0|6076cf2ec42780006af85a96",
                new(2021, 7, 1, 10, 0, 0),
                60,
                "ADDED_NOTES", // the only actual modification, but PUT requires entire object;
                null);
            var expectedDoctorNotes = "ADDED_NOTES";
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            // lets also check with a GET that the modification did in fact apply;
            var resObject = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(appointmentId));
            
            // assert;
            res.Should().BeOfType<NoContentResult>(); // successful PUT should return NO CONTENT per HTTP conventions;
            resObject?
                .DoctorNotes
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(expectedDoctorNotes);
            
            // clean up the DB, i.e. revert it to the original state;
            var appointmentToCleanUp = Context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            appointmentToCleanUp!.DoctorNotes = null;
            await Context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task PutShouldReturnNotFoundResultIfNotFound()
        {
            // arrange;
            // test data set contains this Appointment;
            // empty Guid, no appointment will have that, which is what we want, since we are testing
            // what happens if appointment isn't found;
            var appointmentId = new Guid();
            var command = new Put.Command(
                appointmentId,
                "DOES_NOT_MATTER_AS_APPOINTMENT_ID_DOES_NOT_EXIST",
                DateTime.Today, // does not matter either;
                60, // does not matter either;
                null, // does not matter either;
                null // does not matter either;
            );
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnUnauthorizedIfPatientTriesToAssignAnExistingAppointmentToADifferentDoctor()
        {
            // arrange;
            // appointment with this ID exists in the test data set so can be modified;
            var appointmentId = new Guid("134f3726-3369-492b-b3bd-b62be67b96f8");
            var command = new Put.Command(
                appointmentId,
                "SOME_OTHER_DOCTOR", // the only actual modification, but PUT requires entire object;
                new(2021, 7, 1, 10, 0, 0),
                60,
                null, 
                null);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeOfType<UnauthorizedResult>();
        }
    }
}