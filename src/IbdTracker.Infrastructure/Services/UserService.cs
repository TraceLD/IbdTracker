using System;
using IbdTracker.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;

namespace IbdTracker.Infrastructure.Services
{
    /// <summary>
    /// Service for obtaining information about the currently logged in User.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets currently logged-in User's Auth0 ID.
        /// </summary>
        /// <returns>Currently logged-in User's Auth0 ID.</returns>
        /// 
        /// <exception cref="InvalidOperationException">User is not logged in.</exception>
        string GetUserAuthId();

        /// <summary>
        /// Gets currently logged in user's e-mail from JWT.
        /// Returns <code>null</code> if not logged-in / e-mail not present in JWT.
        /// </summary>
        /// <returns>Currently logged in user's e-mail. <code>null</code> if not logged-in / e-mail not present in JWT.</returns>
        string? GetEmailOrDefault();
    }
    
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Instantiates a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="contextAccessor">The <see cref="HttpContext"/> accessor.</param>
        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc />
        public string GetUserAuthId()
        {
            var user = _contextAccessor.HttpContext.User.Identity?.Name;

            if (user is null)
            {
                throw new InvalidOperationException();
            }

            return user;
        }

        /// <inheritdoc />
        public string? GetEmailOrDefault() =>
            _contextAccessor.HttpContext.User
                .FindFirst(AppJwtClaims.EmailClaim)?
                .Value;
    }
}