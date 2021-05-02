using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.Recommendations;
using IbdTracker.Features.Patients.FoodItems;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.FoodItems
{
    /// <remarks>
    /// This is mostly tested on the Python side.
    ///
    /// These tests require the Python API to be running and be accessible over localhost to pass.
    /// </remarks>
    public class GetRecommendedTests : TestBase
    {
        public GetRecommendedTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldReturnEmptyIfNoMeals()
        {
            IEnumerable<FoodItemRecommendation>? res = null;
            
            await ExecuteInScope(async provider =>
            {
                SetCurrentUser(TestUserHelper.TestNoMealsPatientId);
                
                var mediator = provider.GetRequiredService<IMediator>();
                res = await mediator.Send(new GetRecommended.Query());
            });

            res
                .Should()
                .NotBeNull()
                .And.HaveCount(0);
        }

        [Fact]
        public async Task ShouldReturnGoodRecommendationIfNoPainEventsForMeal()
        {
            // nothing to arrange;

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetRecommended.Query());
            var resList = res!.ToList();
            
            // assert;
            resList
                .Should()
                .HaveCount(2)
                .And.NotContainNulls()
                .And.NotContain(v => v.RecommendationValue < 70);
        }
    }
}