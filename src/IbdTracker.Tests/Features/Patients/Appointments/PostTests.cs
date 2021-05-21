using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Appointments;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.Appointments
{
    public class PostTests : TestBase
    {
        public PostTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPostValidAppointment()
        {
            // arrange;
            const string doctorId = "auth0|6076cf2ec42780006af85a96";
            var startDate = new DateTime(2021, 10, 10);
            const int duration = 60;

            var command = new Post.Command(TestUserHelper.TestPatientId, doctorId,
                startDate, duration, null, null);
            var expected = new AppointmentDto
            {
                PatientId = TestUserHelper.TestPatientId,
                DoctorId = "auth0|6076cf2ec42780006af85a96",
                StartDateTime = startDate,
                DurationMinutes = duration,
                DoctorNotes = null,
                PatientNotes = null
            };

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeEquivalentTo(expected, options => options
                .Excluding(o => o.Location)
                .Excluding(o => o.AppointmentId)
                .Excluding(o => o.DoctorName));

            // clean up;
            var appointmentToRemove =
                await Context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId.Equals(res.AppointmentId));
            Context.Appointments.Remove(appointmentToRemove);
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldFailIfDoctorDoesNotExist()
        {
            // arrange;
            const string doctorId = "THIS_DOCTOR_ID_DOES_NOT_EXIST";
            var startDate = new DateTime(2021, 10, 10);
            const int duration = 60;
            var command = new Post.Command(TestUserHelper.TestPatientId, doctorId,
                startDate, duration, null, null);
            
            // act;
            Func<Task<AppointmentDto>> act = async () =>
                await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);

            // arrange;
            await act
                .Should()
                .ThrowAsync<DbUpdateException>();
        }
    }
}