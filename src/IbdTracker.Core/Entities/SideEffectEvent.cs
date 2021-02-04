using System;

namespace IbdTracker.Core.Entities
{
    public class SideEffectEvent
    {
        public Guid SideEffectEventId { get; set; }
        public Guid PrescriptionId { get; set; }
        public string Description { get; set; } = null!;
    }
}