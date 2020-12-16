namespace IbdTracker.Core.Config
{
    public class DbConfig
    {
        public string Server { get; set; } = null!;
        public int Port { get; set; }
        public string DatabaseName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}