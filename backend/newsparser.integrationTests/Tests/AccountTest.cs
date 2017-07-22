using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NewsParser.API.V1.Models;
using NewsParser.Services;
using NewsParser.IntegrationTests.Fixtures;
using NewsParser.IntegrationTests.Fakes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using NewsParser.Helpers.Utilities;
using NewsParser.Web.Identity.Models;
using newsparser.DAL.Models;

namespace NewsParser.IntegrationTests.Tests
{
    public class AccountResponse
    {
        public AccountModel Data { get; set; }
    }
    
    [Collection("Database collection")]
    public class AccountTest : AbstractIntegrationTest, IDisposable
    {
        private IMailService _mailService;
        private FakeMailService _fakeMailService;

        public AccountTest(TestServerFixture testServerFixture, DatabaseFixture dbFixture) : 
            base(testServerFixture, dbFixture)
        {
           _mailService = ServiceLocator.Instance.GetService(typeof(IMailService)) as IMailService;
           _fakeMailService = _mailService as FakeMailService;
        }

        public void Dispose()
        {
            ClearDatabase();
        }

        [Fact]
        public async Task GetUnauthorized()
        {
            var response = await client.GetAsync("/api/account");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = userRepository.GetUserByEmail(testUser.Email);
            
            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Get, "/api/account");
            var response = await client.SendAsync(request);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<AccountResponse>(responseContentString);

            Assert.Equal(user.Id, responseContent.Data.Id);
            Assert.Equal(user.Email, responseContent.Data.Email);
        }

        [Fact]
        public async Task PostValid()
        {
            var accountModel = new CreateAccountModel
            {
                Email = testUser.Email,
                Password = testUser.Password
            };

            var requestContent = GetJsonContentString(accountModel);
            var response = await client.PostAsync("/api/account", requestContent);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            AssertMailSent(accountModel.Email, "Account Confirmation");

            var createdUser = userRepository.GetUserByEmail(accountModel.Email);
            Assert.NotNull(createdUser);
            Assert.NotNull(createdUser.Password);
            Assert.NotEmpty(createdUser.Password);
        }

        [Fact]
        public async Task PostExistingAccount()
        {
            var accountModel = new CreateAccountModel
            {
                Email = testUser.Email,
                Password = testUser.Password
            };

            await CreateUser(accountModel.Email, accountModel.Password, true);

            var requestContent = GetJsonContentString(accountModel);
            var response = await client.PostAsync("/api/account", requestContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalidemail")]
        public async Task PostInvalidEmail(string email)
        {
            var accountModel = new CreateAccountModel
            {
                Email = email,
                Password = testUser.Password
            };

            var requestContent = GetJsonContentString(accountModel);
            var response = await client.PostAsync("/api/account", requestContent);

            Assert.Equal(422, (int)response.StatusCode);
            await AssertValidationErrors(new List<string> { "Email" }, response);
        }

        [Theory]
        [InlineData("")]
        [InlineData("short")]
        public async Task PostInvalidPassword(string password)
        {
            var accountModel = new CreateAccountModel
            {
                Email = testUser.Email,
                Password = password
            };

            var requestContent = GetJsonContentString(accountModel);
            var response = await client.PostAsync("/api/account", requestContent);

            Assert.Equal(422, (int)response.StatusCode);

            await AssertValidationErrors(new List<string> { "Password" }, response);
        }

        [Fact]
        public async Task PostValidConfirmation()
        {
            await CreateUser(testUser.Email, "tolochko", false);
            var user = authService.FindUserByEmail(testUser.Email);
            string confirmationCode = await authService.GenerateEmailConfirmationTokenAsync(user);
            
            var confirmationModel = new 
            {
                ConfirmationToken = Base64EncodingUtility.Encode(confirmationCode)
            };

            var requestContent = GetJsonContentString(confirmationModel);
            
            var encodedEmail = WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/confirmation", requestContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostAlreadyConfirmedConfirmation()
        {
            await CreateUser(testUser.Email, testUser.Password, true);
            var user = authService.FindUserByEmail(testUser.Email);
            string confirmationCode = await authService.GenerateEmailConfirmationTokenAsync(user);
            
            var confirmationModel = new 
            {
                ConfirmationToken = Base64EncodingUtility.Encode(confirmationCode)
            };

            var requestContent = GetJsonContentString(confirmationModel);
            var encodedEmail = System.Net.WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/confirmation", requestContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostEmptyConfirmation()
        {
            await CreateUser(testUser.Email, testUser.Password, true);
            var user = authService.FindUserByEmail(testUser.Email);
            
            var confirmationModel = new 
            {
                ConfirmationToken = ""
            };

            var requestContent = GetJsonContentString(confirmationModel);
            var encodedEmail = System.Net.WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/confirmation", requestContent);

            Assert.Equal(422, (int)response.StatusCode);
            await AssertValidationErrors(new List<string> { "ConfirmationToken" }, response);
        }

        [Fact]
        public async Task PostNotExistingUserConfirmation()
        {
            var confirmationModel = new 
            {
                ConfirmationToken = testUser.Email
            };

            var requestContent = GetJsonContentString(confirmationModel);
            var encodedEmail = System.Net.WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/confirmation", requestContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostValidPasswordRecoveryRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);
            
            var passwordRecoveryModel = new 
            {
                Email = testUser.Email
            };

            var requestContent = GetJsonContentString(passwordRecoveryModel);
            var response = await client.PostAsync($"/api/account/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertMailSent(passwordRecoveryModel.Email, "Password Recovery");
        }

        [Fact]
        public async Task PostNotConfirmedUserPasswordRecoveryRequest()
        {
            await CreateUser(testUser.Email, testUser.Password, false);
            
            var passwordRecoveryModel = new 
            {
                Email = testUser.Email
            };

            var requestContent = GetJsonContentString(passwordRecoveryModel);
            var response = await client.PostAsync($"/api/account/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostEmptyPasswordRecoveryRequest()
        {
            var passwordRecoveryModel = new 
            {
                Email = ""
            };

            var requestContent = GetJsonContentString(passwordRecoveryModel);
            var response = await client.PostAsync($"/api/account/passwordRecovery", requestContent);

            Assert.Equal(422, (int)response.StatusCode);
            await AssertValidationErrors(new List<string> { "Email" }, response);
        }

        [Fact]
        public async Task PostNotExistingUserPasswordRecoveryRequest()
        {
            var passwordRecoveryModel = new 
            {
                Email = testUser.Email
            };

            var requestContent = GetJsonContentString(passwordRecoveryModel);
            var response = await client.PostAsync($"/api/account/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostValidPasswordResetRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = authService.FindUserByEmail(testUser.Email);

            string passwordRecoveryCode = await authService.GeneratePasswordResetTokenAsync(user);

            var passwordResetModel = new
            {
                PasswordResetToken = Base64EncodingUtility.Encode(passwordRecoveryCode),
                NewPassword = "knopochka"
            };

            var requestContent = GetJsonContentString(passwordResetModel);
            var encodedEmail = WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var accessToken = GetAuthToken(testUser.Email, passwordResetModel.NewPassword);
            Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task PostNotExistingUserPasswordResetRequest()
        {
            var passwordResetModel = new
            {
                PasswordResetToken = Base64EncodingUtility.Encode("some_code"),
                NewPassword = "knopochka"
            };

            var requestContent = GetJsonContentString(passwordResetModel);
            var encodedEmail = WebUtility.UrlEncode("anya.pogorelova@gmail.com");
            var response = await client.PostAsync($"/api/account/{encodedEmail}/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostEmptyPasswordResetRequest()
        {   
            var passwordResetModel = new
            {
                PasswordResetToken = "",
                NewPassword = ""
            };

            var requestContent = GetJsonContentString(passwordResetModel);
            var encodedEmail = WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/passwordRecovery", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(422, (int)response.StatusCode);
            
            await AssertValidationErrors(new List<string> { "NewPassword", "PasswordResetToken" }, response);
        }

        [Fact]
        public async Task PostNotConfirmedUserPasswordResetRequest()
        {
            await CreateUser(testUser.Email, testUser.Password, false);
            var user = authService.FindUserByEmail(testUser.Email);

            string passwordRecoveryCode = await authService.GeneratePasswordResetTokenAsync(user);

            var passwordResetModel = new
            {
                PasswordResetToken = Base64EncodingUtility.Encode(passwordRecoveryCode),
                NewPassword = "knopochka"
            };

            var requestContent = GetJsonContentString(passwordResetModel);
            var encodedEmail = WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostNotValidTokenPasswordResetRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);
            var user = authService.FindUserByEmail(testUser.Email);

            var passwordResetModel = new
            {
                PasswordResetToken = Base64EncodingUtility.Encode("not_valid"),
                NewPassword = "knopochka"
            };

            var requestContent = GetJsonContentString(passwordResetModel);
            var encodedEmail = WebUtility.UrlEncode(testUser.Email);
            var response = await client.PostAsync($"/api/account/{encodedEmail}/passwordRecovery", requestContent);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task PutAccountValid()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var accountUpdateModel = new
            {
                Email = "pohorielova@gmail.com"
            };

            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Put, "/api/account");
            var requestContent = GetJsonContentString(accountUpdateModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var updatedUser = authService.FindUserByEmail(accountUpdateModel.Email);
            Assert.NotNull(updatedUser);
            Assert.Equal(false, updatedUser.EmailConfirmed);

            AssertMailSent(accountUpdateModel.Email, "Email Confirmation");
        }


        [Fact]
        public async Task PutAccountUnauthorized()
        {
            var accountUpdateModel = new
            {
                Email = testUser.Email
            };

            var requestContent = GetJsonContentString(accountUpdateModel);
            var response = await client.PutAsync("/api/account", requestContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PutNotConfirmedAccount()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var accountUpdateModel = new
            {
                Email = "pohorielova@gmail.com"
            };

            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Put, "/api/account");
            
            var user = userRepository.GetUserByEmail(testUser.Email);
            user.EmailConfirmed = false;
            userRepository.UpdateUser(user);

            var requestContent = GetJsonContentString(accountUpdateModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutAccountWithInvalidEmail()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var accountUpdateModel = new
            {
                Email = ""
            };

            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Put, "/api/account");
            var requestContent = GetJsonContentString(accountUpdateModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(422, (int)response.StatusCode);
            await AssertValidationErrors(new List<string> { "Email" }, response);
        }

        [Fact]
        public async Task PutAccountWithTakenEmail()
        {
            await CreateUser(testUser.Email, testUser.Password);

            string otherEmail = "pohorielova@gmail.com";
            await CreateUser(otherEmail, testUser.Password);

            var accountUpdateModel = new
            {
                Email = otherEmail
            };

            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Put, "/api/account");
            var requestContent = GetJsonContentString(accountUpdateModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            var user = userRepository.GetUserByEmail(testUser.Email);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task PasswordChangeValidRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordChangeModel = new
            {
                CurrentPassword = testUser.Password,
                NewPassword = $"{testUser.Password}111"
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Put, 
                "/api/account/passwordChange"
            );
            var requestContent = GetJsonContentString(passwordChangeModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            string accessToken = await GetAuthToken(testUser.Email, passwordChangeModel.NewPassword);
            Assert.NotNull(accessToken);
        }

        [Fact]
        public async Task PasswordChangeUnconfirmedAccountRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordChangeModel = new
            {
                CurrentPassword = testUser.Password,
                NewPassword = $"{testUser.Password}111"
            };

            var request = await CreateAuthorizedRequest(testUser.Email, testUser.Password, HttpMethod.Put, "/api/account/passwordChange");
            
            var user = userRepository.GetUserByEmail(testUser.Email);
            user.EmailConfirmed = false;
            userRepository.UpdateUser(user);
            
            var requestContent = GetJsonContentString(passwordChangeModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PasswordChangeOldPasswordRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordChangeModel = new
            {
                CurrentPassword = testUser.Password,
                NewPassword = testUser.Password
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Put, 
                "/api/account/passwordChange"
            );
            var requestContent = GetJsonContentString(passwordChangeModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(422, (int)response.StatusCode);
            await AssertValidationErrors(new List<string> { "NewPassword" }, response);
        }

        [Fact]
        public async Task PasswordChangeInvalidOldPasswordRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordChangeModel = new
            {
                CurrentPassword = $"{testUser.Password}111",
                NewPassword = "somenewpass"
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Put, 
                "/api/account/passwordChange"
            );
            var requestContent = GetJsonContentString(passwordChangeModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("short")]
        public async Task PasswordChangeInvalidFieldRequest(string newPassword)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordChangeModel = new
            {
                CurrentPassword = testUser.Password,
                NewPassword = newPassword
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Put, 
                "/api/account/passwordChange"
            );
            var requestContent = GetJsonContentString(passwordChangeModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(422, (int)response.StatusCode);

            await AssertValidationErrors(new List<string> { "NewPassword" }, response);
        }

        [Fact]
        public async Task PasswordCreationValidRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordCreationModel = new
            {
                Password = testUser.Password
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/account/passwordCreation");
        
            var user = userRepository.GetUserByEmail(testUser.Email);
            user.Password = "";
            userRepository.UpdateUser(user);

            var requestContent = GetJsonContentString(passwordCreationModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PasswordCreationUnconfirmedRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordCreationModel = new
            {
                Password = testUser.Password
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/account/passwordCreation");

            var user = userRepository.GetUserByEmail(testUser.Email);
            user.Password = "";
            user.EmailConfirmed = false;
            userRepository.UpdateUser(user);

            var requestContent = GetJsonContentString(passwordCreationModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PasswordCreationFailedRequest()
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordCreationModel = new
            {
                Password = testUser.Password
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/account/passwordCreation");

            var requestContent = GetJsonContentString(passwordCreationModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("short")]
        public async Task PasswordCreationInvalidRequest(string newPassword)
        {
            await CreateUser(testUser.Email, testUser.Password);

            var passwordCreationModel = new
            {
                Password = newPassword
            };

            var request = await CreateAuthorizedRequest(
                testUser.Email, 
                testUser.Password, 
                HttpMethod.Post, 
                "/api/account/passwordCreation");

            var requestContent = GetJsonContentString(passwordCreationModel);
            request.Content = requestContent;
            var response = await client.SendAsync(request);

            Assert.Equal(422, (int)response.StatusCode);

            await AssertValidationErrors(new List<string> { "Password" }, response);
        }

        [Fact]
        public async Task PasswordCreationUnauthorizedRequest()
        {
            var passwordCreationModel = new
            {
                Password = testUser.Password
            };

            var requestContent = GetJsonContentString(passwordCreationModel);
            var response = await client.PostAsync("/api/account/passwordCreation", requestContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private void AssertMailSent(string email, string subject)
        {
            var sentEmail = _fakeMailService.Emails.Single(); 
            Assert.Equal(sentEmail.Email, email);
            Assert.Equal(sentEmail.Subject, subject);
            _fakeMailService.Emails.Remove(sentEmail);
        }
    }
}