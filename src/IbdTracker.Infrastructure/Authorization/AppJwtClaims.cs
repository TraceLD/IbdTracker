﻿namespace IbdTracker.Infrastructure.Authorization
{
    /// <summary>
    /// Custom JWT claims for the app.
    /// </summary>
    public static class AppJwtClaims
    {
        // the domain is fake, it's just needed because all custom claims in JWT must be "namespaced" (i.e. have a domain);
        public const string EmailClaim = "https://ibdsymptomtracker.com/claims/email";
    }
}