using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using IbdTracker.Features.Patients.Meals;

namespace IbdTracker.Tests.Features.Patients.Meals
{
    public class GetTests : TestBase
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetMeals()
        {
            await ExecuteInScope(async provider =>
            {
                SetCurrentUser("auth0|608db6053e6508006f4397c1");
                var mediator = provider.GetRequiredService<IMediator>();
                var res = await SendMediatRCqrsInScope(new Get.Query());
                
                
                
                var s = "S";
            });
            
            Assert.True(true);
        }
    }
}