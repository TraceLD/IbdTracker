using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;

namespace IbdTracker.Infrastructure.Services
{
    /// <summary>
    /// Service for interacting with Auth0's machine to machine APIs.
    /// </summary>
    public interface IAuth0Service
    {
        /// <summary>
        /// Registers a user as a patient.
        /// </summary>
        /// <param name="auth0UserId">User's Auth0 ID.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterPatient(string auth0UserId);
        
        /// <summary>
        /// Registers a user as an unverified doctor.
        /// </summary>
        /// <param name="auth0UserId">User's Auth0 ID.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task RegisterDoctor(string auth0UserId);
        
        /// <summary>
        /// Marks a user as a verified doctor.
        /// </summary>
        /// <param name="auth0UserId">User's Auth0 ID.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        Task MarkDoctorAsVerified(string auth0UserId);
    }
    
    /// <inheritdoc />
    public class Auth0Service : IAuth0Service
    {
        private readonly ManagementApiClient _managementApiClient;

        private const string UnverifiedDoctorRoleId = "rol_Y1x5sWDoMnpytY3j";
        private const string DoctorRoleId = "rol_Kla2rnKKLf5AO63n";
        private const string PatientRoleId = "rol_nOyC1aDUPbfngGtj";
        
        /// <summary>
        /// Instantiates a new instance of <see cref="Auth0Service"/>.
        /// </summary>
        /// <param name="managementApiClient">The Auth0 Management API HTTP client.</param>
        public Auth0Service(ManagementApiClient managementApiClient)
        {
            _managementApiClient = managementApiClient;
        }

        /// <inheritdoc />
        public async Task RegisterPatient(string auth0UserId) =>
            await _managementApiClient.Users.AssignRolesAsync(auth0UserId,
                new AssignRolesRequest {Roles = new[] {PatientRoleId}});
        
        /// <inheritdoc />
        public async Task RegisterDoctor(string auth0UserId) =>
            await _managementApiClient.Users.AssignRolesAsync(auth0UserId,
                new AssignRolesRequest {Roles = new[] {UnverifiedDoctorRoleId}});

        /// <inheritdoc />
        public async Task MarkDoctorAsVerified(string auth0UserId)
        {
            await _managementApiClient.Users.RemoveRolesAsync(auth0UserId,
                new AssignRolesRequest {Roles = new[] {UnverifiedDoctorRoleId}});
            await _managementApiClient.Users.AssignRolesAsync(auth0UserId,
                new AssignRolesRequest {Roles = new[] {DoctorRoleId}});
        }
    }
}