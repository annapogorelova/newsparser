using System;
using NewsParser.IntegrationTests.Fixtures;
using Xunit;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using NewsParser.IntegrationTests.Fakes;
using NewsParser.FeedParser.Services;
using NewsParser.DAL.Repositories.Feed;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using NewsParser.API.V1.Models;

namespace NewsParser.IntegrationTests.Tests
{
    [Collection("Database collection")]
    public class FeedTest: AbstractIntegrationTest, IDisposable
    {
        private IFeedRepository _feedRepository { get; set; }
        public FeedTest(TestServerFixture testServerFixture, DatabaseFixture dbFixture) : 
            base(testServerFixture, dbFixture)
        {
            _feedRepository = new FeedRepository(dbContext);
        }

        public void Dispose()
        {
            ClearDatabase();
        }

        [Fact]
        public async Task GetUnauthorized()
        {
            var response = await client.GetAsync("/api/feed");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(5, "sky")]
        [InlineData(10, "", new string[] { "sky","road","cat" })]
        public async Task Get(int pageSize, string search = null, string[] tags = null)
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var channelModel = new 
            {
                FeedUrl = "http://backend.photos.tooleks.com/rss.xml",
                IsPrivate = false
            };

            var requestContent = GetJsonContentString(channelModel);
            var channelCreateRequest = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                "/api/channels"
            );
            channelCreateRequest.Content = requestContent;
            var channelResponse = await client.SendAsync(channelCreateRequest);
            Assert.Equal(HttpStatusCode.Created, channelResponse.StatusCode);
            
            var tagsString = tags != null ? tags.Join(",") : "";

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password,
                HttpMethod.Get,
                $"/api/feed?pageSize={pageSize}&search={search ?? string.Empty}&tags={tagsString}"
            );
            var response = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContent = await GetListResponseContent<FeedItemModel>(response);
            var userFeed = _feedRepository.GetPage(0, pageSize, user.Id, search, null, tags);
            Assert.Equal(userFeed.Count(), responseContent.Data.Count);
        }

        [Fact]
        public async Task GetUnsubscribed()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);

            var channelModel = new 
            {
                FeedUrl = "http://backend.photos.tooleks.com/rss.xml",
                IsPrivate = false
            };

            var requestContent = GetJsonContentString(channelModel);
            var channelCreateRequest = await CreateAuthorizedRequest(
                testUser.Email,
                testUser.Password,
                HttpMethod.Post,
                "/api/channels"
            );
            channelCreateRequest.Content = requestContent;
            var channelResponse = await client.SendAsync(channelCreateRequest);
            Assert.Equal(HttpStatusCode.Created, channelResponse.StatusCode);
            
            var channelResponseContent = await GetResponseContent<ChannelSubscriptionModel>(channelResponse);

            var otherEmail = "tooleks@gmail.com";
            await CreateUser(otherEmail, testUser.Password);

            var feedRequest = await CreateAuthorizedRequest(
                otherEmail,
                testUser.Password,
                HttpMethod.Get,
                $"/api/feed?sources={channelResponseContent.Data.Id}"
            );
            var feedResponse = await client.SendAsync(feedRequest);
            Assert.Equal(HttpStatusCode.OK, feedResponse.StatusCode);

            var feedResponseContent = await GetListResponseContent<FeedItemModel>(feedResponse);
            Assert.Empty(feedResponseContent.Data);
        }
    }
}