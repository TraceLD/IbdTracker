using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.FoodItems;
using Xunit;

namespace IbdTracker.Tests.Features.FoodItems
{
    public class GetByIdTests : TestBase
    {
        public GetByIdTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetExistingFiById()
        {
            // arrange;
            // test data set contains this FI;
            var fiGuid = new Guid("007f91f1-779c-48d1-88dd-7a8aedd820a4");
            var expected = new FoodItemDto
            {
                Name = "Meat bouillon",
                PictureUrl = null,
                FoodItemId = fiGuid
            };
            
            // act;
            var res = await SendMediatorRequestInScope(new GetById.Query(fiGuid));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task ShouldReturnNullIfNotFound()
        {
            // arrange;
            // test data set does not contain this FI;
            // empty Guid, no BM will have that, which is what we want, since we are testing
            // what happens if appointment isn't found;
            var fiGuid = new Guid();
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetById.Query(fiGuid));

            // assert;
            res.Should().BeNull();
        }
    }
}