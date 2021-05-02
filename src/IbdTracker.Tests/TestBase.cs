using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IbdTracker.Core;
using IbdTracker.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IbdTracker.Tests
{
    /// <summary>
    /// Base class that all tests inherit.
    /// </summary>
    [Collection("SharedFixture")]
    public abstract class TestBase : IDisposable
    {
        private readonly IServiceScope _serviceScope;

        /// <summary>
        /// Instantiates a new instance of <see cref="TestBase"/>.
        /// </summary>
        /// <param name="sharedFixture">The shared fixture.</param>
        protected TestBase(SharedFixture sharedFixture)
        {
            SharedFixture = sharedFixture;
            _serviceScope = CreateScope();
            Services = _serviceScope.ServiceProvider;
            Configuration = GetService<IConfiguration>();
            Context = GetService<IbdSymptomTrackerContext>();
            HttpContextAccessor = GetService<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext = new DefaultHttpContext { RequestServices = Services };
            UserService = GetService<IUserService>();
        }

        /// <summary>
        /// The database context.
        /// </summary>
        protected IbdSymptomTrackerContext Context { get; }
        
        /// <summary>
        /// The user service.
        /// </summary>
        protected IUserService UserService { get; }

        /// <summary>
        /// The configuration for tests.
        /// </summary>
        private IConfiguration Configuration { get; }
        
        /// <summary>
        /// The <see cref="HttpContext"/> that mimics the ASP.NET Core environment's request context as closely as possible.
        /// </summary>
        private HttpContext HttpContext => HttpContextAccessor.HttpContext!;
        
        /// <summary>
        /// The <see cref="HttpContext"/> accessor.
        /// </summary>
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        /// <summary>
        /// The IoC container, i.e. the service provider.
        /// </summary>
        private IServiceProvider Services { get; }
        
        /// <summary>
        /// The fixture.
        /// </summary>
        private SharedFixture SharedFixture { get; }

        /// <summary>
        /// Disposes of <see cref="TestBase"/>.
        /// </summary>
        public virtual void Dispose()
            => _serviceScope.Dispose();

        /// <summary>
        /// Executes functions in a new IoC scope. This mimics very closely HTTP requests in ASP.NET Core.
        /// For each request coming in a new scope is created.
        /// </summary>
        /// <param name="function">Function to execute in a new scope.</param>
        protected async Task ExecuteInScope(Func<IServiceProvider, Task> function)
        {
            using var scope = CreateScope();
            await function(scope.ServiceProvider);
        }

        /// <summary>
        /// Sends (executes) a MediatR (<see cref="IMediator"/>) query/command in a new IoC scope.
        /// This closely mimics our controllers where we a new scope is created for each request that
        /// comes in and then the work is passed off to MediatR.
        /// </summary>
        /// <param name="request">The command/query.</param>
        /// <typeparam name="TResponse">The response type.</typeparam>
        /// <returns>The result of the command/query.</returns>
        protected async Task<TResponse?> SendMediatorRequestInScope<TResponse>(IRequest<TResponse?> request)
        {
            var response = default(TResponse);
            
            await ExecuteInScope(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                response = await mediator.Send(request);
            });
            
            return response;
        }

        protected async Task<TResponse?> SendMediatorRequestInScopeOnBehalfOfTheTestPatient<TResponse>(
            IRequest<TResponse?> request, bool includeEmailInUserClaims = false)
        {
            var response = default(TResponse);
            
            await ExecuteInScope(async sp =>
            {
                SetCurrentUser(TestUserHelper.TestPatientId);
                
                var mediator = sp.GetRequiredService<IMediator>();
                response = await mediator.Send(request);
            });
            
            return response;
        }

        /// <summary>
        /// Gets a service from the IoC container, i.e. the service provider (<see cref="IServiceProvider"/>). 
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service.</returns>
        protected T GetService<T>()
            where T : notnull
            => Services.GetRequiredService<T>();

        protected void GetService<T>(out T service)
            where T : notnull
            => service = Services.GetRequiredService<T>();

        /// <summary>
        /// Sets the current user to a given username. This mimics the username we get
        /// from the OAuth 2.0 provider, Auth0, in the JWT (JSON Web Token).
        /// </summary>
        /// <param name="username">The username.</param>
        /// <remarks>This can be set inside <see cref="ExecuteInScope"/>, to set the user for that scope only.</remarks>
        protected void SetCurrentUser(string username)
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username)
            }));
        }

        /// <summary>
        /// Sets the current user to a given 
        /// </summary>
        protected void SetCurrentUserToPatientTestUser()
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", TestUserHelper.TestPatientId)
            }));
        }

        /// <summary>
        /// Creates a new scope.
        /// </summary>
        /// <returns>The new scope.</returns>
        private IServiceScope CreateScope()
            => SharedFixture.Services.CreateScope();
    }
}