namespace IbdTracker.Core
{
    /// <summary>
    /// Enumerates the account type.
    /// </summary>
    public enum AccountType
    {
        Unregistered = -1,
        Unknown = 0,
        Patient = 1,
        Doctor = 2,
        UnverifiedDoctor = 3
    }
}