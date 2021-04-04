using System;

namespace IbdTracker.Core.CommonDtos
{
    public class GlobalNotificationDto
    {
        public Guid GlobalNotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string TailwindColour { get; set; } = null!;
        public string? Url { get; set; }
    }
}