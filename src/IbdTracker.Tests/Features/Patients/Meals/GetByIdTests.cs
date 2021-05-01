using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using Xunit;
using IbdTracker.Features.Patients.Meals;

namespace IbdTracker.Tests.Features.Patients.Meals
{
    public class GetByIdTests : TestBase
    {
        public GetByIdTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetById()
        {
            // arrange;
            // test data set contains the following meal:
            var mealId = new Guid("482911cf-2418-461c-b195-5524d9b803e2");
            var expected = new MealDto
            {
                MealId = mealId,
                Name = "Nutella sandwich",
                PatientId = TestUserHelper.TestPatientId,
                FoodItems = new()
                {
                    new()
                    {
                        FoodItemId = new("57cc9e00-1bfc-4e0b-8df2-ba41baf8e4a9"), Name = "Nutella",
                        PictureUrl = null
                    },
                    new()
                    {
                        FoodItemId = new("b2f8d933-8897-4783-a356-00db9b28a778"), Name = "White bread",
                        PictureUrl = null
                    }
                }
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetById.Query(mealId));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnNullIfNotFound()
        {
            // arrange;
            // empty Guid, no meal will have that, which is what we want, since we are testing
            // what happens if meal isn't found;
            var mealId = new Guid();
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetById.Query(mealId));
            
            // assert;
            res.Should().BeNull();
        }
    }
}