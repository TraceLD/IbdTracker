using System;
using System.Threading.Tasks;
using FluentAssertions;
using IbdTracker.Infrastructure.Services;
using Xunit;

namespace IbdTracker.Tests.Services
{
    public class UserServiceTests : TestBase
    {
        private readonly IUserService _userService;
        
        public UserServiceTests(SharedFixture sharedFixture) : base(sharedFixture)
        {
            _userService = GetService<IUserService>();
        }

        [Fact]
        public void ShouldGetAuthIdWhenLoggedIn()
        {
            // arrange;
            const string userId = "testId";
            SetCurrentUser(userId);
            
            // act;
            var res = _userService.GetUserAuthId();
            
            // assert;
            res.Should().BeEquivalentTo(userId);
        }

        [Fact]
        public void ShouldGetAuthIdThrowWhenNotLoggedIn()
        {
            // act;
            Func<string> act = () => _userService.GetUserAuthId();

            // assert;
            act
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void ShouldGetCorrectEmailIfLoggedInAndEmailClaimPresent()
        {
            // arrange;
            const string userId = "testId";
            const string email = "test@test.com";
            SetCurrentUser(userId, email);
            
            // act;
            var res = _userService.GetEmailOrDefault();
            
            // assert;
            res.Should().BeEquivalentTo(email);
        }

        [Fact]
        public void ShouldGetNullEmailIfNotLoggedIn()
        {
            // act;
            var res = _userService.GetEmailOrDefault();
            
            // assert;
            res.Should().BeNull();
        }

        [Fact]
        public void ShouldGetNullEmailIfLoggedInAndEmailClaimNotPresent()
        {
            // arrange;
            const string userId = "testId";
            SetCurrentUser(userId);
            
            // act;
            var res = _userService.GetEmailOrDefault();
            
            // assert;
            res.Should().BeNull();
        }

        [Fact]
        public void IsRegisteredCheckShouldThrowIfNoAuth0()
        {
            // act;
            Func<Task<bool>> act = async () => await _userService.IsRegistered();

            // assert;
            act
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task IsRegisteredCheckShouldReturnFalseIfRegistrationNotFullyCompletedButAuth0Present()
        {
            // arrange;
            const string auth0Id = "testId";
            SetCurrentUser(auth0Id);
            
            // act;
            var res = await _userService.IsRegistered();
            
            // assert;
            res.Should().BeFalse();
        }

        [Fact]
        public async Task IsRegisteredShouldReturnTrueForRegisteredPatient()
        {
            // arrange;
            SetCurrentUserToPatientTestUser();
            
            // act;
            var res = await _userService.IsRegistered();
            
            // assert;
            res.Should().BeTrue();
        }

        [Fact]
        public async Task IsRegisteredShouldReturnTrueForRegisteredDoctor()
        {
            // arrange;
            SetCurrentUserToDoctorTestUser();
            
            // act;
            var res = await _userService.IsRegistered();
            
            // assert;
            res.Should().BeTrue();
        }
    }
}