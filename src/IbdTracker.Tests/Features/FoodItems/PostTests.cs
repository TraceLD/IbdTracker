using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.FoodItems;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.FoodItems
{
    public class PostTests : TestBase
    {
        public PostTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPostValidFi()
        {
            // arrange;
            const string name = "TEST_FI";
            var command = new Post.Command(name, null);
            var expected = new FoodItemDto
            {
                Name = name,
                PictureUrl = null
            };
            
            // act;
            var res = await SendMediatorRequestInScope(command);
            
            // assert;
            res
                .Should()
                .BeEquivalentTo(expected, o => o.Excluding(o => o.FoodItemId));
            
            // clean up;
            var fiToRemove = await Context.FoodItems
                .FirstOrDefaultAsync(fi => fi.FoodItemId == res!.FoodItemId);
            Context.FoodItems.Remove(fiToRemove);
            await Context.SaveChangesAsync();
        }
    }
}