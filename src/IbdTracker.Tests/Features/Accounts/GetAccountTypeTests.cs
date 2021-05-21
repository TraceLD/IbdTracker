using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Core;
using IbdTracker.Features.Accounts;
using Xunit;

namespace IbdTracker.Tests.Features.Accounts
{
    public class GetAccountTypeTests : TestBase
    {
        public GetAccountTypeTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
        }

        [Fact]
        public async Task ShouldReturnUnregisteredForAnUnregisteredId()
        {
            // arrange;
            const string authId = "THIS_ID_DOES_NOT_EXIST_IN_THE_TEST_DATA_SET";
            
            // act;
            var res = await SendMediatorRequestInScope(new GetAccountType.Query(authId));
            
            // assert;
            res.Should().Be(AccountType.Unregistered);
        }
        
        [Fact]
        public async Task ShouldReturnPatientForAPatientId()
        {
            // arrange;
            var authId = TestUserHelper.TestPatientId;
            
            // act;
            var res = await SendMediatorRequestInScope(new GetAccountType.Query(authId));
            
            // assert;
            res.Should().Be(AccountType.Patient);
        }
        
        [Fact]
        public async Task ShouldReturnDoctorForADoctorId()
        {
            // arrange;
            var authId = TestUserHelper.TestDoctorId;
            
            // act;
            var res = await SendMediatorRequestInScope(new GetAccountType.Query(authId));
            
            // assert;
            res.Should().Be(AccountType.Doctor);
        }
    }
}