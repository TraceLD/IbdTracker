using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.Entities;
using IbdTracker.Features.FoodItems;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.FoodItems
{
    public class DeleteTests : TestBase
    {
        public DeleteTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldDeleteFiIfFound()
        {
            // arrange;
            // prepare a fi to delete;
            const string name = "TEST_ITEM";
            var fiToDelete = new FoodItem(name);
            await Context.FoodItems.AddAsync(fiToDelete);
            await Context.SaveChangesAsync();
            
            // prepare the CQRS command;
            var command = new Delete.Command(fiToDelete.FoodItemId);

            var res = await SendMediatorRequestInScope(command);
            var resObj = await SendMediatorRequestInScope(new GetById.Query(fiToDelete.FoodItemId));
            
            // assert;
            res.Should().BeOfType<NoContentResult>();
            resObj.Should().BeNull();
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfNotFound()
        {
            // arrange;
            // empty GUID as we are testing to see what happens
            // when a matching fi can't be found in the database;
            var idOfTheFi = new Guid();
            var command = new Delete.Command(idOfTheFi);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }
    }
}