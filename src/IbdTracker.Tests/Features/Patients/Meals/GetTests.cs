using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
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
            // arrange;
            // test data set contains the following 2 meals:
            var expected = new List<MealDto>
            {
                new()
                {
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
                    },
                    MealId = new("482911cf-2418-461c-b195-5524d9b803e2"),
                    Name = "Nutella sandwich",
                    PatientId = TestUserHelper.TestPatientId
                },
                new()
                {
                    FoodItems = new()
                    {
                        new()
                        {
                            FoodItemId = new("791a9f14-d703-4fde-aa9d-45aa2990c776"), Name = "Pizza", PictureUrl = null
                        }
                    },
                    MealId = new("ae8cbc6f-0468-4d2b-8443-eca56f60f46b"),
                    Name = "Precooked Pizza",
                    PatientId = TestUserHelper.TestPatientId
                }
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new Get.Query());

            // assert;
            res
                .Should()
                .HaveCount(2)
                .And.BeEquivalentTo(expected);
        }
    }
}