using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core.CommonDtos;
using IbdTracker.Features.Patients.BowelMovements;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.BowelMovements
{
    public class GetOneTests : TestBase
    {
        public GetOneTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetExistingBm()
        {
            // arrange;
            // test data set contains this BM;
            var bmGuid = new Guid("f8cc02c9-9fb4-4b5f-9611-e10b301233ff");
            var expected = new BowelMovementEventDto
            {
                BowelMovementEventId = bmGuid,
                PatientId = TestUserHelper.TestPatientId,
                ContainedBlood = true,
                ContainedMucus = true,
                DateTime = new DateTime(2021, 5, 1, 15, 0, 0)
            };
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(bmGuid));
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnNullIfNotFound()
        {
            // arrange;
            // test data set does not contain this BM;
            // empty Guid, no BM will have that, which is what we want, since we are testing
            // what happens if appointment isn't found;
            var bmGuid = new Guid();
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new GetOne.Query(bmGuid));

            // assert;
            res.Should().BeNull();
        }
    }
}