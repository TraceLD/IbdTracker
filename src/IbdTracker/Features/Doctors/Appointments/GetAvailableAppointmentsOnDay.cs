using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IbdTracker.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IbdTracker.Features.Doctors.Appointments
{
    public class GetAvailableAppointmentsOnDay
    {
        // do not have to validate for null as it comes from ASP.NET Core query and route args which are validated;
        public record Query(string DoctorId, DateTime AppointmentDayAsDateTime) : IRequest<Result>;
        public record Result(string DoctorId, DateTime Day, IList<DateTime> AvailableAppointmentTimesOnDayUtc);

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IbdSymptomTrackerContext _context;

            public Handler(IbdSymptomTrackerContext context)
            {
                _context = context;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var (doctorId, appointmentDayAsDateTime) = request;
                appointmentDayAsDateTime = DateTime.SpecifyKind(appointmentDayAsDateTime, DateTimeKind.Utc);

                if (appointmentDayAsDateTime <= DateTime.UtcNow.Date)
                {
                    return new(request.DoctorId, appointmentDayAsDateTime, new List<DateTime>());
                }
                
                // get office hours for that day of week;
                var dayOfWeek = appointmentDayAsDateTime.DayOfWeek;
                var officeHours = (await _context.Doctors
                    .Where(d => d.DoctorId.Equals(request.DoctorId))
                    .Select(d => d.OfficeHours)
                    .FirstOrDefaultAsync(cancellationToken))?
                        .FirstOrDefault(oh => oh.DayOfWeek == dayOfWeek);
                
                // if has no office hours on that day then that means they do not take any appointments on that day of the week
                // so return empty list;
                if (officeHours is null)
                {
                    return new(request.DoctorId, appointmentDayAsDateTime, new List<DateTime>());
                }
                
                // get already booked appointments for that doctor for that day;
                var appointmentsOnDay = await _context.Appointments.Where(a =>
                        a.DoctorId.Equals(doctorId) && a.StartDateTime.Date.Equals(appointmentDayAsDateTime))
                    .ToListAsync(cancellationToken);
                
                var freeTimes = new List<DateTime>();
                var startDate = new DateTime(appointmentDayAsDateTime.Year, appointmentDayAsDateTime.Month,
                    appointmentDayAsDateTime.Day, (int) officeHours.StartTimeUtc.Hour,
                    (int) officeHours.StartTimeUtc.Minutes, 0, DateTimeKind.Utc);
                var endDate = new DateTime(appointmentDayAsDateTime.Year, appointmentDayAsDateTime.Month,
                    appointmentDayAsDateTime.Day, (int) officeHours.EndTimeUtc.Hour,
                    (int) officeHours.EndTimeUtc.Minutes, 0, DateTimeKind.Utc);
                
                // create DateTimes;
                var iDate = startDate;
                while (iDate < endDate)
                {
                    freeTimes.Add(iDate);
                    iDate = iDate.AddMinutes(30);
                }

                if (appointmentsOnDay is null || !appointmentsOnDay.Any())
                {
                    return new Result(request.DoctorId, appointmentDayAsDateTime, freeTimes);
                }

                var appointmentDates = appointmentsOnDay
                    .Select(a => DateTime.SpecifyKind(a.StartDateTime, DateTimeKind.Utc))
                    .ToList();
                
                // subtract booked appointment times from free appointment times;
                freeTimes = freeTimes
                    .Except(appointmentDates)
                    .ToList();
                return new(request.DoctorId, appointmentDayAsDateTime, freeTimes);
            }
        }
    }
}