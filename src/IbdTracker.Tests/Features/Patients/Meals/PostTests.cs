using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.Meals;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.Meals
{
    public class PostTests : TestBase
    {
        public PostTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPostValidMeal()
        {
            // arrange;
            var command = new Post.Command("TestMeal", new List<Guid> {new("54fc903b-b4a7-480c-ba5c-824ad1c15ffe")});
            var expected = new MealDto
            {
                Name = "TestMeal",
                PatientId = TestUserHelper.TestPatientId
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeEquivalentTo(expected, options => options
                .Excluding(o => o.MealId)
                .Excluding(o => o.FoodItems));
            
            // clean up;
            var mealToRemove = await Context.Meals.FirstOrDefaultAsync(m => m.MealId.Equals(res.MealId));
            Context.Meals.Remove(mealToRemove);
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldPostMealWithSameNameBecauseMealNamesAreNotUnique()
        {
            // arrange;
            var command = new Post.Command("Nutella sandwich", new List<Guid> {new("54fc903b-b4a7-480c-ba5c-824ad1c15ffe")});
            var expected = new MealDto
            {
                Name = "Nutella sandwich",
                PatientId = TestUserHelper.TestPatientId
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeEquivalentTo(expected, options => options
                .Excluding(o => o.MealId)
                .Excluding(o => o.FoodItems));
            
            // clean up;
            var mealToRemove = await Context.Meals.FirstOrDefaultAsync(m => m.MealId.Equals(res.MealId));
            Context.Meals.Remove(mealToRemove);
            await Context.SaveChangesAsync();
        }
    }
}