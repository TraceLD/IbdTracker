using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using IbdTracker.Features.Patients.AppSettings;

namespace IbdTracker.Tests.Features.Patients.AppSettings
{
    public class GetTests : TestBase
    {
        public GetTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldGetPatientAppSettings()
        {
            // arrange;
            // the test patient has the default value of "true" set for if they want to share data for research;
            var expected = new Get.Result(TestUserHelper.TestPatientId, true);
            
            // act;
            var res = await SendMediatorRequestInScopeOnBehalfOfTheTestPatient(new Get.Query());
            
            // assert;
            res.Should().BeEquivalentTo(expected);
        }
    }
}