using System;

namespace IbdTracker.Core.Entities
{
    public class GlobalNotification
    {
        public Guid GlobalNotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string TailwindColour { get; set; } = null!;
        public string? Url { get; set; }

        public GlobalNotification(
            string title,
            string message,
            string tailwindColour,
            string? url = null
        )
        {
            Title = title;
            Message = message;
            TailwindColour = tailwindColour;
            Url = url;
        }
    }
}