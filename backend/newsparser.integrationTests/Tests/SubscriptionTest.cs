using System;
using NewsParser.IntegrationTests.Fixtures;
using Xunit;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace NewsParser.IntegrationTests.Tests
{
    [Collection("Database collection")]
    public class SubscriptionTest : AbstractIntegrationTest, IDisposable
    {
        public SubscriptionTest(TestServerFixture testServerFixture, DatabaseFixture dbFixture) : 
            base(testServerFixture, dbFixture)
        {

        }

        public void Dispose()
        {
            ClearDatabase();
        }

        [Fact]
        public async Task PostValid()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            var createdChannel = channelRepository.Add(GetTestChannel());

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostUnauthorized()
        {
            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                $"/api/subscription/1");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PostNotExistingChannel()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                $"/api/subscription/1");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostPrivateByOtherUser()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);

            var createdChannel = channelRepository.Add(GetTestChannel());
            AddChannelToUser(otherUser, createdChannel, true);
            
            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostExistingSubscription()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var createdChannel = channelRepository.Add(GetTestChannel());
            AddChannelToUser(user, createdChannel);

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteValid(bool isPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var createdChannel = channelRepository.Add(GetTestChannel());
            AddChannelToUser(user, createdChannel, isPrivate);

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var channelInDb = channelRepository.GetById(createdChannel.Id);
            if(isPrivate)
            {
                Assert.Null(channelInDb);
            }
            else
            {
                Assert.NotNull(channelInDb);
            }
        }

        [Fact]
        public async Task DeleteUnauthorized()
        {
            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/1");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteNotExisting()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/1");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOtherUsersPrivate()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);

            var createdChannel = channelRepository.Add(GetTestChannel());
            AddChannelToUser(otherUser, createdChannel, true);

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteNotSubscribed()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var createdChannel = channelRepository.Add(GetTestChannel());

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteOwnPrivateWithOtherSubscribers(bool isOtherSubscriptionPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);

            var createdChannel = channelRepository.Add(GetTestChannel());
            AddChannelToUser(user, createdChannel, true);
            AddChannelToUser(otherUser, createdChannel, isOtherSubscriptionPrivate);

            var request = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Delete,
                $"/api/subscription/{createdChannel.Id}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var channelInDb = channelRepository.GetById(createdChannel.Id);
            Assert.NotNull(channelInDb);
        }
    }
}