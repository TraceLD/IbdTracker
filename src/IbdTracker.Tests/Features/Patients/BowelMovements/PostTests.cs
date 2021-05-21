using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.BowelMovements;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.BowelMovements
{
    public class PostTests : TestBase
    {
        public PostTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPostValidBm()
        {
            // arrange;
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0);
            var command = new Post.Command(dateTime, true, true);
            var expected = new BowelMovementEventDto
            {
                ContainedBlood = true,
                ContainedMucus = true,
                DateTime = dateTime,
                PatientId = TestUserHelper.TestPatientId
            };

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            
            // assert;
            res
                .Should()
                .BeEquivalentTo(expected, options => options.Excluding(o => o.BowelMovementEventId));

            // clean up;
            var bmToRemove = await Context.BowelMovementEvents
                .FirstOrDefaultAsync(bm => bm.BowelMovementEventId.Equals(res!.BowelMovementEventId));
            Context.BowelMovementEvents.Remove(bmToRemove);
            await Context.SaveChangesAsync();
        }
    }
}