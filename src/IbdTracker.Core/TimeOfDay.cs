namespace IbdTracker.Core
{
    public class TimeOfDay
    {
        public uint Hour { get; set; }
        public uint Minutes { get; set; }

        public override string ToString() => $"{Hour}:{Minutes}";
    }
}