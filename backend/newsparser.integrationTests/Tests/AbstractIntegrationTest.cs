using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewsParser.Web.Auth;
using NewsParser.DAL;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Channels;
using NewsParser.DAL.Repositories.Users;
using NewsParser.Helpers.ActionFilters.ModelValidation;
using NewsParser.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace NewsParser.IntegrationTests.Tests
{
    public class AuthResponse
    {
        public string token_type;
        public string access_token;
        public string expires_in;
        public string refresh_token;
    }

    public class ApiResponse<T>
    {
        public T Data { get; set; }
    }

    public class ApiListResponse<T>
    {
        public List<T> Data { get; set; }
        public int Total { get; set; }
    }

    public class ValidationResponse
    {
        public string Message { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
    }

    public class UserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public abstract class AbstractIntegrationTest: IClassFixture<TestServerFixture>
    {
        public readonly HttpClient client;
        public readonly TestServer server;
        protected IUserRepository userRepository;
        protected IChannelRepository channelRepository;
        protected IAuthService authService;
        protected AppDbContext dbContext;
        protected IConfiguration configuration;
        protected UserCredentials testUser { get; set; }
        
        protected AbstractIntegrationTest(
            TestServerFixture testServerFixture, 
            DatabaseFixture dbFixture)
        {
            client = testServerFixture.client;
            server = testServerFixture.server;
            dbContext = dbFixture.CreateDbContext();
            userRepository = new UserRepository(dbContext);
            channelRepository = new ChannelRepository(dbContext);
            authService = ServiceLocator.Instance.GetService(typeof(IAuthService)) as IAuthService;
            configuration = ServiceLocator.Instance.GetService(typeof(IConfiguration)) as IConfiguration;
            testUser = new UserCredentials 
            { 
                Email = "anya.pogorelova@gmail.com",
                Password = "tolochko"
            };
        }

        protected StringContent GetJsonContentString(object content)
        {
            var serializedContent = JsonConvert.SerializeObject(content);
            var requestContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            return requestContent;
        }

        protected StringContent GetFormContentString(object content)
        {
            var requestContent = new StringContent(content.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            return requestContent;
        }

        protected IConfiguration GetConfiguration()
        {
            var configuration = ServiceLocator.Instance.GetService(typeof(IConfiguration)) as IConfiguration;
            return configuration;
        }

        protected async Task<string> GetAuthToken(string email, string password)
        {
            var response = await PostAuthRequest(email, password);
            var responseContentString = await response.Content.ReadAsStringAsync();
            AuthResponse responseContent = JsonConvert.DeserializeObject<AuthResponse>(responseContentString);
            return responseContent.access_token;
        }

        protected async Task CreateUser(string email, string password, bool confirmed = true)
        {
            await authService.CreateAsync(email, password);
            if(confirmed)
            {
                var user = userRepository.GetUserByEmail(email);
                user.EmailConfirmed = true;
                userRepository.UpdateUser(user);
            }
        }

        protected async Task<HttpResponseMessage> PostAuthRequest(
            string email, 
            string password, 
            string grant_type = "password", 
            string scope = null)
        {
            var credentialsString = GetAuthRequestBody(email, password, grant_type, scope);
            var requestContent = GetFormContentString(credentialsString);  
            var response = await client.PostAsync("/api/token", requestContent);
            return response;
        }

        private string GetAuthRequestBody(
            string email, 
            string password, 
            string grant_type = "password", 
            string scope = null)
        {
            var requestBody = $@"username={email}&password={password}&grant_type={grant_type}";
            if(!string.IsNullOrEmpty(scope))
            {
                requestBody += $"&scope={scope}";
            }
            return requestBody;
        }

        protected async Task<HttpRequestMessage> CreateAuthorizedRequest(
            string email, string password, HttpMethod httpMethod, string apiRoute)
        {
            var authToken = await GetAuthToken(email, password);
            var requestMessage = new HttpRequestMessage(httpMethod, apiRoute);
            requestMessage.Headers.Add("Authorization", $"Bearer {authToken}");
            return requestMessage;
        }

        protected void ClearDatabase()
        {
            Console.WriteLine("-------------------------- Clearing db -------------------------------");
            dbContext.Database.ExecuteSqlCommand("delete from channels_feed_items");
            dbContext.Database.ExecuteSqlCommand("delete from tags_feed_items");
            dbContext.Database.ExecuteSqlCommand("delete from user_channels");
            dbContext.Database.ExecuteSqlCommand("delete from user_external_ids");
            dbContext.Database.ExecuteSqlCommand("delete from tokens");
            dbContext.Database.ExecuteSqlCommand("delete from users");
            dbContext.Database.ExecuteSqlCommand("delete from channels");
            dbContext.Database.ExecuteSqlCommand("delete from feed_items");
            dbContext.Database.ExecuteSqlCommand("delete from tags");
            
        }

        protected async Task AssertValidationErrors(List<string> fields, HttpResponseMessage response)
        {
            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ValidationResponse>(responseContentString);
            
            Assert.NotNull(responseJson.Message);
            Assert.NotNull(responseJson.ValidationErrors);

            var validationErrorFields = responseJson.ValidationErrors.Select(e => e.Field).ToList();
            foreach(var field in fields)
            {
                Assert.Contains(field, validationErrorFields);
            }
        }

        protected async Task<ApiResponse<T>> GetResponseContent<T>(HttpResponseMessage response)
        {
            var responseContetString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContetString);
            return responseContent;
        }

        protected async Task<ApiListResponse<T>> GetListResponseContent<T>(HttpResponseMessage response)
        {
            var responseContetString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ApiListResponse<T>>(responseContetString);
            return responseContent;
        }

        protected List<Channel> GetTestChannels()
        {
            var channels = new List<Channel>()
            {
                new Channel
                {
                    FeedUrl = "https://habrahabr.ru/rss/all/",
                    Name = "Хабрахабр / Все публикации",
                    Description = "Все публикации на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "https://geektimes.ru/rss/all/",
                    Name = "Geektimes / Все публикации",
                    Description = "Все публикации на Geektimes",
                    ImageUrl = "https://geektimes.ru/images/logo.png",
                    WebsiteUrl = "https://geektimes.ru/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "https://css-tricks.com/feed/",
                    Name = "CSS-Tricks",
                    Description = "Tips, Tricks, and Techniques on using Cascading Style Sheets.",
                    WebsiteUrl = "https://css-tricks.com/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "http://arzamas.academy/feed_v1.rss",
                    Name = "Arzamas | Всё",
                    Description = "Новые курсы каждый четверг и дополнительные материалы в течение недели",
                    ImageUrl = "http://arzamas.academy/apple-touch-icon.png",
                    WebsiteUrl = "http://arzamas.academy/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "https://habrahabr.ru/rss/flows/develop/all/",
                    Name = "Хабрахабр / Все публикации в потоке Разработка",
                    Description = "Все публикации в потоке Разработка на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "https://habrahabr.ru/rss/hub/programming/",
                    Name = "Хабрахабр / Программирование / Интересные публикации",
                    Description = "Интересные публикации из хаба «Программирование» на Хабрахабре",
                    ImageUrl = "https://habrahabr.ru/images/logo.png",
                    WebsiteUrl = "https://habrahabr.ru/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "https://postnauka.ru/feed",
                    Name = "ПостНаука",
                    Description = "все, что вы хотели знать о науке, но не знали, у кого спросить",
                    WebsiteUrl = "https://postnauka.ru/",
                    FeedFormat = FeedFormat.RSS
                },
                new Channel
                {
                    FeedUrl = "http://backend.photos.tooleks.com/rss.xml",
                    Name = "Photos by Oleksandr Tolochko",
                    Description = "Photos by Oleksandr Tolochko",
                    WebsiteUrl = "http://backend.photos.tooleks.com",
                    FeedFormat = FeedFormat.RSS
                }
            };

            return channels;
        }

        protected Channel GetTestChannel()
        {
            return new Channel
            {
                FeedUrl = "http://backend.photos.tooleks.com/rss.xml",
                Name = "Photos by Oleksandr Tolochko",
                Description = "Photos by Oleksandr Tolochko",
                WebsiteUrl = "http://backend.photos.tooleks.com",
                FeedFormat = FeedFormat.RSS
            };
        }

        protected List<Channel> CreateTestChannels()
        {
            var channels = GetTestChannels();
            foreach (var channel in channels)
            {
                channelRepository.Add(channel);
            }

            return channelRepository.GetAll()
                .OrderByDescending(s => s.Users.Count())
                .ThenBy(s => s.Name)
                .ToList();
        }

        protected void AddChannelsToUser(User user, List<Channel> channels, bool isPrivate = false)
        {
            foreach(var channel in channels)
            {
                AddChannelToUser(user, channel, isPrivate);
            }
        }

        protected void AddChannelToUser(User user, Channel channel, bool isPrivate = false)
        {
            var existingChannel = channelRepository.GetByUrl(channel.FeedUrl);
            channelRepository.AddUser(new UserChannel 
            { 
                ChannelId = existingChannel.Id, 
                UserId = user.Id,
                IsPrivate = isPrivate
            });
        }
    }
}