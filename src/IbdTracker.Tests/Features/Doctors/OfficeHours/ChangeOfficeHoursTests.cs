using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Doctors.OfficeHours;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Doctors.OfficeHours
{
    public class ChangeOfficeHoursTests : TestBase
    {
        public ChangeOfficeHoursTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldSucceedWithValidOfficeHours()
        {
            // arrange;
            var newOfficeHours = new List<Core.OfficeHours>
            {
                new()
                {
                    DayOfWeek = DayOfWeek.Monday,
                    EndTimeUtc = new() {Hour = 3, Minutes = 0},
                    StartTimeUtc = new() {Hour = 1, Minutes = 0}
                }
            };

            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(
                new ChangeOfficeHours.Command(newOfficeHours));
            var resObj = await SendMediatorRequestInScopeOnBehalfOfTheTestDoctor(new Get.Query());
            
            // assert;
            res.Should().BeOfType<NoContentResult>();
            resObj.Should().BeEquivalentTo(newOfficeHours);

            // clean up;
            (await Context.Doctors
                    .FirstOrDefaultAsync(d => d.DoctorId.Equals(TestUserHelper.TestDoctorId)))
                .OfficeHours = new List<Core.OfficeHours>();
            await Context.SaveChangesAsync();
        } 
    }
}