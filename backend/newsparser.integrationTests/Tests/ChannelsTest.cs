using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NewsParser.API.Models;
using NewsParser.DAL.Models;
using NewsParser.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace NewsParser.IntegrationTests.Tests
{
    [Collection("Database collection")]
    public class ChannelsTest : AbstractIntegrationTest, IDisposable
    {
        public ChannelsTest(TestServerFixture testServerFixture, DatabaseFixture dbFixture) : 
            base(testServerFixture, dbFixture)
        {
        }

        public void Dispose()
        {
            ClearDatabase();
        }

        [Fact]
        public async Task GetUnauthorized()
        {
            var response = await client.GetAsync("/api/channels");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetAll()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var channels = CreateTestChannels();
            AddChannelsToUser(user, channels.Take(4).ToList());
            
            int pageSize = 5;
            
            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels?pageSize={pageSize}"
            );
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetListResponseContent<ChannelModel>(response);
            Assert.Equal(channels.Count, responseContent.Total);
            
            var responseList = responseContent.Data;
            Assert.Equal(pageSize, responseList.Count);
        }

        [Fact]
        public async Task GetSubscribed()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            int subscibedCount = 6;
            var channels = CreateTestChannels();
            AddChannelsToUser(user, channels.Take(subscibedCount).ToList());
            
            int pageSize = 5;

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels?pageSize={pageSize}&subscribed=true");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetListResponseContent<ChannelModel>(response);
            Assert.Equal(subscibedCount, responseContent.Total);
            
            var responseList = responseContent.Data;
            Assert.Equal(pageSize, responseList.Count);
        }

        [Fact]
        public async Task GetWithSearch()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var channels = CreateTestChannels();
            AddChannelsToUser(user, channels.Take(4).ToList());
            
            int pageSize = 5;
            string search = "Хабрахабр";
            var channelsToBeFound = channels.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            
            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels?pageSize={pageSize}&search={search}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetListResponseContent<ChannelModel>(response);
            Assert.Equal(channelsToBeFound.Count(), responseContent.Total);
            
            var responseList = responseContent.Data;
            Assert.True(responseList.First().Name.ToLower().Contains(search.ToLower()));
        }

        [Fact]
        public async Task GetAllWithOtherUsersPrivateChannel()
        {
            var channels = CreateTestChannels();
            
            int targetUserChannelsCount = 4;

            await CreateUser(testUser.Email, testUser.Password);
            var targetUser = userRepository.GetUserByEmail(testUser.Email);
            AddChannelsToUser(targetUser, channels.Take(targetUserChannelsCount).ToList());

            // Add private channels to the other user
            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);
            var privateChannels = channels.Skip(targetUserChannelsCount).Take(1).ToList();
            AddChannelsToUser(otherUser, privateChannels, true);

            int pageSize = 5;

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels?pageSize={pageSize}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetListResponseContent<ChannelModel>(response);
            var responseList = responseContent.Data;
            var privateChannelsInReponse = responseList.Where(c => c.Id == privateChannels.First().Id);
            Assert.Empty(privateChannelsInReponse);
        }

        [Fact]
        public async Task GetById()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var channels = CreateTestChannels();
            AddChannelsToUser(user, channels);

            int channelId = channels.First().Id;
            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels/{channelId}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            var channel = responseContent.Data;
            Assert.Equal(channelId, channel.Id);
        }

        [Fact]
        public async Task GetNotExistingById()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels/1");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetNotSubscribedById()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);
            var channels = CreateTestChannels();

            int channelId = channels.First().Id;

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels/{channelId}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            var channel = responseContent.Data;
            Assert.Equal(channelId, channel.Id);
        }

        [Fact]
        public async Task GetPrivateByOtherUserById()
        {
            var channels = CreateTestChannels();

            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);
            AddChannelsToUser(otherUser, channels.Take(1).ToList(), true);

            int channelId = channels.First().Id;

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels/{channelId}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPrivateByTwoUsersById()
        {
            var channels = CreateTestChannels();

            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);
            AddChannelsToUser(user, channels.Take(1).ToList(), true);

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);
            AddChannelsToUser(otherUser, channels.Take(1).ToList(), true);

            int channelId = channels.First().Id;

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Get, 
                $"/api/channels/{channelId}");
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            var channel = responseContent.Data;
            Assert.Equal(channelId, channel.Id);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostValid(bool isPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var channelModel = new
            {
                FeedUrl = "http://backend.photos.tooleks.com/rss.xml",
                IsPrivate = isPrivate
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/channels");
            var requestContent = GetJsonContentString(channelModel);
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            Assert.Equal(responseContent.Data.FeedUrl, channelModel.FeedUrl);
            Assert.Equal(responseContent.Data.IsPrivate, channelModel.IsPrivate);
            Assert.True(responseContent.Data.IsSubscribed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostExistingWithPublicSubscription(bool isPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var existingChannel = channelRepository.Add(GetTestChannel());

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);
            AddChannelToUser(otherUser, existingChannel);

            var channelModel = new
            {
                FeedUrl = existingChannel.FeedUrl,
                IsPrivate = isPrivate
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/channels");
            var requestContent = GetJsonContentString(channelModel);
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            Assert.Equal(responseContent.Data.FeedUrl, channelModel.FeedUrl);
            Assert.False(responseContent.Data.IsPrivate);
            Assert.True(responseContent.Data.IsSubscribed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostExistingWithPrivateSubscription(bool isPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var existingChannel = channelRepository.Add(GetTestChannel());

            string otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);
            var otherUser = userRepository.GetUserByEmail(otherEmail);
            AddChannelToUser(otherUser, existingChannel, true);

            var channelModel = new
            {
                FeedUrl = existingChannel.FeedUrl,
                IsPrivate = isPrivate
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/channels");
            var requestContent = GetJsonContentString(channelModel);
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            Assert.Equal(responseContent.Data.FeedUrl, channelModel.FeedUrl);
            Assert.Equal(responseContent.Data.IsPrivate, isPrivate);
            Assert.True(responseContent.Data.IsSubscribed);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task PostExistingWithNoSubscriptions(bool isPrivate)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var existingChannel = channelRepository.Add(GetTestChannel());
            var channelModel = new
            {
                FeedUrl = existingChannel.FeedUrl,
                IsPrivate = isPrivate
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password,
                HttpMethod.Post, 
                "/api/channels");
            var requestContent = GetJsonContentString(channelModel);
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await GetResponseContent<ChannelSubscriptionModel>(response);
            Assert.Equal(responseContent.Data.FeedUrl, channelModel.FeedUrl);
            Assert.Equal(responseContent.Data.IsPrivate, isPrivate);
            Assert.True(responseContent.Data.IsSubscribed);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidurl.com")]
        public async Task PostInvalid(string feedUrl)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var channelModel = new
            {
                FeedUrl = feedUrl,
                IsPrivate = false
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/channels");
            var requestContent = GetJsonContentString(channelModel);
            request.Content = requestContent;

            var response = await client.SendAsync(request);
            Assert.Equal(422, (int)response.StatusCode);

            await AssertValidationErrors(new List<string> { "FeedUrl" }, response);
        }
    }
}