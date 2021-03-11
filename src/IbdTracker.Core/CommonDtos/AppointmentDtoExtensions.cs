using IbdTracker.Core.Entities;

namespace IbdTracker.Core.CommonDtos
{
    public static class AppointmentDtoExtensions
    {
        public static AppointmentDto ToDto(this Appointment appointment)
        {
            return new()
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor.Name,
                StartDateTime = appointment.StartDateTime,
                DurationMinutes = appointment.DurationMinutes,
                Location = appointment.Doctor.Location,
                DoctorsNotes = appointment.DoctorsNotes,
                PatientsNotes = appointment.PatientsNotes
            };
        }
    }
}