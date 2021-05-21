using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.Entities;
using IbdTracker.Features.Patients.BowelMovements;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.BowelMovements
{
    public class DeleteTests : TestBase
    {
        public DeleteTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldDeleteBmIfFound()
        {
            // arrange;
            // prepare a bm to delete;
            var bmToDelete = new BowelMovementEvent
            {
                PatientId = TestUserHelper.TestPatientId,
                ContainedBlood = true,
                ContainedMucus = true,
                DateTime = DateTime.Now
            };
            await Context.BowelMovementEvents.AddAsync(bmToDelete);
            await Context.SaveChangesAsync();
            
            // prepare the CQRS command;
            var command = new Delete.Command(bmToDelete.BowelMovementEventId);
            var getObjectQuery = new GetOne.Query(bmToDelete.BowelMovementEventId);
            
            // res should be of type NoContentResult;
            // resObject should return null because the object no longer exists (deleted);

            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            var resObject = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(getObjectQuery);
            
            // assert;
            res.Should().BeOfType<NoContentResult>();
            resObject.Should().BeNull();
            
            // no need to clean up the DB as we've just deleted the BM we've added;
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfNotFound()
        {
            // arrange;
            // empty GUID as we are testing to see what happens
            // when a matching BM can't be found in the database;
            var idOfTheBm = new Guid();
            var command = new Delete.Command(idOfTheBm);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }
    }
}