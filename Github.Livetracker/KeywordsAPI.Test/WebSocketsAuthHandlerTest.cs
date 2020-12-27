using Moq;
using NUnit.Framework;
using SettingsProxyAPI.Auth;
using SettingsProxyAPI.Auth.WebSockets;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace KeywordsAPI.Test
{
    [TestFixture]
    public class WebSocketsAuthHandlerTest
    {
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void SetUp()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Test]
        public void CorrectToken_SuccessfulAuthentication()
        {
            // Arrange
            string accessToken = "TEST_TOKEN";
            string tokenHash = accessToken.GetTokenHash();
            User user = new User { TokenHash = tokenHash };

            _mockUserRepository.Setup(ur => ur.GetUserByHashedTokenAsync(tokenHash))
                .Returns(Task.FromResult(user));
            WebSocketsAuthHandler webSocketsAuthHandler = new WebSocketsAuthHandler(_mockUserRepository.Object);

            // Act, Assert
            IObservable<User> userObservable = webSocketsAuthHandler.IdentifyUser(accessToken);

            userObservable.Do(onNext: u => Assert.That(u.TokenHash.Equals(tokenHash))).Subscribe();
        }

        [Test]
        public void AccessTokenIsEmptyString_FailedAuthentication()
        {
            // Arrange
            string accessToken = string.Empty;

            WebSocketsAuthHandler webSocketsAuthHandler = new WebSocketsAuthHandler(_mockUserRepository.Object);

            // Act, Assert
            IObservable<User> userObservable = webSocketsAuthHandler.IdentifyUser(accessToken);

            TestDelegate testDelegate = () => userObservable.Do(
                onNext: u => { },
                onError: ex =>
                {
                    Assert.That(ex is UnauthorizedAccessException);
                    Assert.That(ex.Message.Equals("Access token is an empty string"));
                })
                .Subscribe();
            Assert.Throws(typeof(UnauthorizedAccessException), testDelegate);
        }

        [Test]
        public void WrongToken_FailedAuthentication()
        {
            // Arrange
            string correctUserToken = "CORRECT_TOKEN";
            string correctUserTokenHash = correctUserToken.GetTokenHash();
            User user = new User { TokenHash = correctUserTokenHash };

            _mockUserRepository.Setup(ur => ur.GetUserByHashedTokenAsync(correctUserTokenHash))
                .Returns(Task.FromResult(user));
            WebSocketsAuthHandler webSocketsAuthHandler = new WebSocketsAuthHandler(_mockUserRepository.Object);

            // Act, Assert
            IObservable<User> userObservable = webSocketsAuthHandler.IdentifyUser("WRONG_TOKEN");

            TestDelegate testDelegate = () => userObservable.Do(
                onNext: u => { },
                onError: ex =>
                {
                    Assert.That(ex is UnauthorizedAccessException);
                    Assert.That(ex.Message.Equals("User with such token was not found"));
                })
                .Subscribe();
            Assert.Throws(typeof(UnauthorizedAccessException), testDelegate);
        }
    }
}