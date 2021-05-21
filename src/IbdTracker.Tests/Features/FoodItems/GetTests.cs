using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.FoodItems;
using Xunit;

namespace IbdTracker.Tests.Features.FoodItems
{
    public class GetTests : TestBase
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldReturnFis()
        {
            // arrange;
            var query = new Get.Query();
            
            // act;
            var res = await SendMediatorRequestInScope(query);
            
            // assert;
            res.Should()
                .NotBeNullOrEmpty();
        }
    }
}