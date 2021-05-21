using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.BowelMovements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.BowelMovements
{
    public class PutTests : TestBase
    {
        public PutTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPutAValidBm()
        {
            // arrange;
            // test data set contains this BM;
            var bmGuid = new Guid("f8cc02c9-9fb4-4b5f-9611-e10b301233ff");
            var dateTime = new DateTime(2021, 5, 1, 15, 0, 0);
            var command = new Put.Command(bmGuid, dateTime, true, false);

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            var resObj = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(bmGuid));
            
            // assert;
            res.Should().BeOfType<NoContentResult>();
            resObj?
                .ContainedMucus
                .Should()
                .BeFalse();
            
            // clean up the DB, i.e. revert it to the original state;
            var bmToCleanUp = await Context.BowelMovementEvents
                .FirstOrDefaultAsync(bm => bm.BowelMovementEventId == bmGuid);
            bmToCleanUp!.ContainedMucus = true;
            await Context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldReturnNotFoundResultIfNotFound()
        {
            // arrange;
            // test data set does not contain this BM;
            // empty Guid, no BM will have that, which is what we want, since we are testing
            // what happens if appointment isn't found;
            var bmGuid = new Guid();
            var command = new Put.Command(bmGuid, DateTime.UtcNow, false, false);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);

            // assert;
            res.Should().BeOfType<NotFoundResult>();
        }
    }
}