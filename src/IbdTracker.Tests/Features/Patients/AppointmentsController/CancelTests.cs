using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.Entities;
using IbdTracker.Features.Patients.Appointments;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.AppointmentsController
{
    public class CancelTests : TestBase
    {
        public CancelTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldCancelAnAppointmentIfFound()
        {
            // arrange;
            // prepare an appointment to cancel;
            var appointmentToCancel = new Appointment(
                TestUserHelper.TestPatientId,
                "auth0|6076cf2ec42780006af85a96",
                new(2021, 10, 10, 15, 0, 0),
                60,
                "SOME_DOCTOR_NOTES",
                "SOME_PATIENT_NOTES");
            await Context.Appointments.AddAsync(appointmentToCancel);
            await Context.SaveChangesAsync();
            
            // prepare the CQRS MediatR (mediator) command;
            var command = new Cancel.Command(appointmentToCancel.AppointmentId);
            var getObjectQuery = new GetOne.Query(appointmentToCancel.AppointmentId);
            
            // res should be of type NoContentResult;
            // resObject should return null because the object no longer exists (cancelled);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            var resObject = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(getObjectQuery);
            
            // assert;
            res.Should().BeOfType<NoContentResult>();
            resObject.Should().BeNull();
            
            // no need to clean up the DB as we've just cancelled (deleted) the appointment;
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfNotFound()
        {
            // arrange;
            // empty GUID as we are testing to see what happens
            // when a matching appointment can't be found in the database;
            var idOfTheAppointmentToCancel = new Guid();
            var command = new Cancel.Command(idOfTheAppointmentToCancel);
            // result should be of type NotFoundResult;
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }
    }
}