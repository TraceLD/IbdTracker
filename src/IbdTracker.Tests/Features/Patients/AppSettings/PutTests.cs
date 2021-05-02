using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Features.Patients.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IbdTracker.Tests.Features.Patients.AppSettings
{
    public class PutTests : TestBase
    {
        public PutTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldPutValidSettings()
        {
            // arrange;
            // the value in the test data set is "true", so we will change it to "false";
            const bool startingValue = true;
            const bool expectedNewValue = false; // + expected return type from command = NoContentResult;
            var command = new Put.Command(false);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(command);
            var newValue = await Context.PatientApplicationSettings
                .Where(s => s.PatientId.Equals(TestUserHelper.TestPatientId))
                .FirstOrDefaultAsync();

            // assert;
            res.Should().BeOfType<NoContentResult>();
            newValue.ShareDataForResearch.Should().Be(expectedNewValue);
            
            // clean up the db;
            // reset to starting value;
            newValue.ShareDataForResearch = startingValue;
            await Context.SaveChangesAsync();
        }
    }
}