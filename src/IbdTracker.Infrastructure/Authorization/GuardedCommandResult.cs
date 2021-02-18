namespace IbdTracker.Infrastructure.Authorization
{
    public class GuardedCommandResult<T>
    {
        public T? Payload { get; set; }
        public bool AuthSucceeded { get; set; }

        public GuardedCommandResult()
        {
            AuthSucceeded = false;
        }

        public GuardedCommandResult(T payload)
        {
            Payload = payload;
            AuthSucceeded = true;
        }
    }
}