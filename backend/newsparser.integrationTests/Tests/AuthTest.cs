using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using NewsParser.Auth;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Users;
using NewsParser.Identity.Models;
using NewsParser.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using Xunit;

namespace NewsParser.IntegrationTests.Tests
{
    [Collection("Database collection")]
    public class AuthTest: AbstractIntegrationTest, IDisposable
    {
        public class UserCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public AuthTest(TestServerFixture testServerFixture, DatabaseFixture dbFixture):
            base(testServerFixture, dbFixture)
        {
        }

        public void Dispose()
        {
            ClearDatabase();
        }

        [Fact]
        public async void PostValidAuthRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            var response = await PostAuthRequest(testUser.Email, testUser.Password);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContentString = await response.Content.ReadAsStringAsync();
            AuthResponse responseContent = JsonConvert.DeserializeObject<AuthResponse>(responseContentString);
            
            Assert.Equal("Bearer", responseContent.token_type);
            Assert.NotEmpty(responseContent.access_token);
            Assert.NotNull(responseContent.access_token);
            
            var config = GetConfiguration();
            var tokenLifeTime = int.Parse(config.GetSection("Security")["AccessTokenLifetimeMinutes"]);
            
            Assert.Equal(tokenLifeTime*60, int.Parse(responseContent.expires_in));
        }

        [Fact]
        public async void PostValidAuthRequestOfflineAccessScope()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            var response = await PostAuthRequest(testUser.Email, testUser.Password, "password", "offline_access");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var responseContentString = await response.Content.ReadAsStringAsync();
            AuthResponse responseContent = JsonConvert.DeserializeObject<AuthResponse>(responseContentString);
            
            Assert.Equal("Bearer", responseContent.token_type);
            Assert.NotEmpty(responseContent.access_token);
            Assert.NotNull(responseContent.access_token);
            Assert.NotEmpty(responseContent.refresh_token);
            Assert.NotNull(responseContent.refresh_token);
            
            var config = GetConfiguration();
            var tokenLifeTime = int.Parse(config.GetSection("Security")["AccessTokenLifetimeMinutes"]);
            
            Assert.Equal(tokenLifeTime*60, int.Parse(responseContent.expires_in));
        }

        [Fact]
        public async void PostInvalidAuthRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var response = await PostAuthRequest(testUser.Email, "invalidpass");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostEmptyRequestBody()
        {
            var requestContent = GetFormContentString("");  
            var response = await client.PostAsync("/api/token", requestContent);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid_grant_type")]
        public async void PostInvalidGrantTypeRequest(string grantType)
        {
            var response = await PostAuthRequest(testUser.Email, testUser.Password, "password", grantType);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostNotExistingUserAuthRequest()
        {
            var response = await PostAuthRequest(testUser.Email, testUser.Password);   
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostNotVerifiedUserAuthRequest()
        {
            await CreateUser(testUser.Email, testUser.Password, false);
            var response = await PostAuthRequest(testUser.Email, testUser.Password);            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void PostAuthWithRefreshTokenRequest()
        { 
            await CreateUser(testUser.Email, testUser.Password);

            var tokenResponse = await PostAuthRequest(
                testUser.Email, 
                testUser.Password, 
                "password", 
                "offline_access"
            );
            Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);
            
            var responseContentString = await tokenResponse.Content.ReadAsStringAsync();
            AuthResponse responseContent = JsonConvert.DeserializeObject<AuthResponse>(responseContentString);

            Assert.Equal("Bearer", responseContent.token_type);
            Assert.NotEmpty(responseContent.access_token);
            Assert.NotNull(responseContent.access_token);
            Assert.NotEmpty(responseContent.refresh_token);
            Assert.NotNull(responseContent.refresh_token);
            
            var config = GetConfiguration();
            var tokenLifeTime = int.Parse(config.GetSection("Security")["AccessTokenLifetimeMinutes"]);
            
            Assert.Equal(tokenLifeTime*60, int.Parse(responseContent.expires_in));

            // Request for token with refresh token
            var refreshTokenResponse = await PostRefreshAuthRequest(responseContent.refresh_token);
            Assert.Equal(HttpStatusCode.OK, tokenResponse.StatusCode);

            var refreshTokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
            AuthResponse refreshTokenResponseContent = JsonConvert.DeserializeObject<AuthResponse>(refreshTokenResponseString);

            Assert.Equal("Bearer", refreshTokenResponseContent.token_type);
            Assert.NotEmpty(refreshTokenResponseContent.access_token);
            Assert.NotNull(refreshTokenResponseContent.access_token);
            Assert.NotEmpty(refreshTokenResponseContent.refresh_token);
            Assert.NotNull(refreshTokenResponseContent.refresh_token);

            Assert.Equal(tokenLifeTime*60, int.Parse(refreshTokenResponseContent.expires_in));
        }

        private async Task<HttpResponseMessage> PostRefreshAuthRequest(string refresh_token, string scope = "offline_access")
        {
            var requestBodyString = GetRefreshAuthRequestBody(refresh_token, scope);
            var requestContent = GetFormContentString(requestBodyString);  
            var response = await client.PostAsync("/api/token", requestContent);
            return response;
        }

        private string GetRefreshAuthRequestBody(string refresh_token, string scope)
        {
            var requestBody = $"grant_type=refresh_token&refresh_token={refresh_token}";
            if(!string.IsNullOrEmpty(scope))
            {
                requestBody += $"&scope={scope}";
            }
            return requestBody;
        }
    }
}
