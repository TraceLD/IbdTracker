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
    [Collection("SharedFixture")]
    public abstract class TestBase : IDisposable
    {
        private readonly IServiceScope _serviceScope;

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

        protected IbdSymptomTrackerContext Context { get; }
        protected IUserService UserService { get; }

        private IConfiguration Configuration { get; }
        private HttpContext HttpContext => HttpContextAccessor.HttpContext!;
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IServiceProvider Services { get; }
        private SharedFixture SharedFixture { get; }

        public virtual void Dispose()
            => _serviceScope.Dispose();

        protected async Task ExecuteInScope(Func<IServiceProvider, Task> function)
        {
            using var scope = CreateScope();
            await function(scope.ServiceProvider);
        }

        protected async Task<TResponse> SendMediatorRequestInScope<TResponse>(IRequest<TResponse> request)
        {
            var response = default(TResponse);
            
            await ExecuteInScope(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                response = await mediator.Send(request);
            });
            
            return response;
        }

        protected async Task<TResponse> SendMediatorRequestInScopeOnBehalfOfTheTestPatient<TResponse>(
            IRequest<TResponse> request, bool includeEmailInUserClaims = false)
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

        protected T GetService<T>()
            where T : notnull
            => Services.GetRequiredService<T>();

        protected void GetService<T>(out T service)
            where T : notnull
            => service = Services.GetRequiredService<T>();

        protected void SetCurrentUser(string username)
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", username)
            }));
        }

        protected void SetCurrentUserToPatientTestUser()
        {
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", TestUserHelper.TestPatientId)
            }));
        }

        private IServiceScope CreateScope()
            => SharedFixture.Services.CreateScope();
    }
}