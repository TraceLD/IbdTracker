using System;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Checks whether the currently logged-in Auth0 user has completed
        /// the IBD Symptom Tracker registration process completely and has an account registered
        /// in the database.
        /// </summary>
        /// <returns>Whether the currently logged-in Auth0 user has completed the registration for IBD Symptom Tracker.</returns>
        ///
        /// <exception cref="InvalidOperationException">User is not logged in.</exception>
        Task<bool> IsRegistered();
    }
    
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IbdSymptomTrackerContext _context;

        /// <summary>
        /// Instantiates a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="contextAccessor">The <see cref="HttpContext"/> accessor.</param>
        /// <param name="context">The <see cref="IbdSymptomTrackerContext"/> database context.</param>
        public UserService(IHttpContextAccessor contextAccessor, IbdSymptomTrackerContext context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
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

        /// <inheritdoc />
        public async Task<bool> IsRegistered()
        {
            var user = GetUserAuthId();

            if (user is null)
            {
                throw new InvalidOperationException();
            }
            
            var existsPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId.Equals(user));

            if (existsPatient is not null)
            {
                return true;
            }

            var existsDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId.Equals(user) && d.IsApproved);

            return existsDoctor is not null;
        }
    }
}