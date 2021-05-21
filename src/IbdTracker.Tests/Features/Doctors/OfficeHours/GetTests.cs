using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Doctors.OfficeHours;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors.OfficeHours
{
    public class GetTests : TestBase 
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldReturnEmptyIfNoOfficeHoursSet()
        {
            // nothing to arrange;
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query());
            
            // assert;
            res.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnCorrectOfficeHoursIfTheyAreSet()
        {
            // arrange;
            var expected = new List<Core.OfficeHours>
            {
                new()
                {
                    DayOfWeek = DayOfWeek.Monday,
                    EndTimeUtc = new() {Hour = 3, Minutes = 0},
                    StartTimeUtc = new() {Hour = 1, Minutes = 0}
                }
            };
            var doc = await Context.Doctors.FirstOrDefaultAsync(d => d.DoctorId.Equals(TestUserHelper.TestDoctorId));
            doc.OfficeHours = expected;
            await Context.SaveChangesAsync();
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query());
            
            // assert;
            res.Should().BeEquivalentTo(expected);

            doc.OfficeHours = new List<Core.OfficeHours>();
            await Context.SaveChangesAsync();
        }
    }
}