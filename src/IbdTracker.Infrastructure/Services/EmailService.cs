using System;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Core.Config;
using IbdTracker.Core.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace IbdTracker.Infrastructure.Services
{
    /// <summary>
    /// Service to send automated e-mail messages from the IBD tracker's corporate email.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email message from IBD Tracker's email.
        /// </summary>
        /// <param name="to">Recipient's email address.</param>
        /// <param name="subject">Subject of the message.</param>
        /// <param name="htmlBody">Body of the message in the HTML format.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendMessage(string to, string subject, string htmlBody);

        /// <summary>
        /// Sends an email confirming an appointment has been booked.
        /// </summary>
        /// <param name="appointment">Appointment details.</param>
        /// <param name="to">Recipient's email address.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendAppointmentConfirmationEmail(Appointment appointment, string to);
    }
    
    /// <inheritdoc />
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        private readonly IbdSymptomTrackerContext _context;
        private readonly TimeZoneInfo _localTimeZone;
        private readonly TimeZoneInfo _utcTimeZone;

        /// <summary>
        /// Instantiates a new instance of <see cref="EmailService"/>.
        /// </summary>
        /// <param name="emailConfig">The application's email config.</param>
        /// <param name="context">The db context.</param>
        public EmailService(EmailConfig emailConfig, IbdSymptomTrackerContext context)
        {
            _emailConfig = emailConfig;
            _context = context;
            _localTimeZone = TimeZoneInfo.Local;
            _utcTimeZone = TimeZoneInfo.Utc;
        }

        /// <inheritdoc />
        public async Task SendMessage(string to, string subject, string htmlBody)
        {
            var emailMsg = new MimeMessage();
            emailMsg.From.Add(MailboxAddress.Parse(_emailConfig.Address));
            emailMsg.To.Add(MailboxAddress.Parse(to));
            emailMsg.Subject = subject;
            emailMsg.Body = new TextPart(TextFormat.Html) {Text = htmlBody};

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
            await smtpClient.SendAsync(emailMsg);
            await smtpClient.DisconnectAsync(true);
        }

        /// <inheritdoc />
        public async Task SendAppointmentConfirmationEmail(Appointment appointment, string to)
        {
            var date = DateTime.SpecifyKind(appointment.StartDateTime, DateTimeKind.Utc);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorId.Equals(appointment.DoctorId));
            
            var html = $@"
<h1>IBDtracker - IBD Symptom Tracker</h1>

<h2>Appointment nr {appointment.AppointmentId} booked!</h2>
<h3>Your appointment has been booked.</h3>

<b>{doctor.Name}</b>
<br>
<br>
<b>{date.ToLocalTime():g} {_localTimeZone.StandardName} ({date:t} {_utcTimeZone.StandardName})</b>
<p>{doctor.Location}</p>
<p>Duration: {appointment.DurationMinutes} minutes</p>";

            await SendMessage(to, $"Appointment booking confirmation (nr {appointment.AppointmentId})", html);
        }
    }
}