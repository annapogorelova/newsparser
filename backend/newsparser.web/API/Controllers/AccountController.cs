using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.Auth;
using NewsParser.BL.Services.Users;
using NewsParser.Cache;
using NewsParser.Helpers.ActionFilters.ModelValidation;
using NewsParser.Helpers.Utilities;
using NewsParser.Identity.Models;
using NewsParser.Services;

namespace NewsParser.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserBusinessService _userBusinessService;
        private readonly IMailService _mailService;

        public AccountController(IAuthService authService, IMailService mailService, IUserBusinessService userBusinessService)
        {
            _authService = authService;
            _mailService = mailService;
            _userBusinessService = userBusinessService;
        }

        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [Cache(Duration = 60, DeferByUser = true)]
        public JsonResult Get()
        {
            var user = _authService.GetCurrentUser();
            var userModel = Mapper.Map<AccountModel>(user);
            return new JsonResult(new { data = userModel });
        }

        [HttpPost]
        [ValidateModel]
        public async Task<JsonResult> Post([FromBody]CreateAccountModel model)
        {
            var result = await _authService.CreateAsync(model.Email, model.Password);
            
            if (result.Succeeded)
            {
                var user = _authService.FindUserByEmail(model.Email);
                string confirmationCode = await _authService.GenerateEmailConfirmationTokenAsync(user);
                await _mailService.SendAccountConfirmationEmail(user.Email, Base64EncodingUtility.Encode(confirmationCode));
                return MakeSuccessResponse(HttpStatusCode.Created, "Account was created");
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to create the account", 
                result
            );
        }

        [HttpPost("{email}/confirmation")]
        [ValidateModel]
        public async Task<JsonResult> Post(string email, [FromBody]EmailConfirmationModel model)
        {
            var user = _authService.FindUserByEmail(email);
            
            if(user.EmailConfirmed)
            {
                return MakeErrorResponse(HttpStatusCode.Forbidden, "Email has already been confirmed");
            }

            var result = await _authService.ConfirmEmail(user, Base64EncodingUtility.Decode(model.ConfirmationToken));
            
            if(result.Succeeded)
            {
                return MakeSuccessResponse(HttpStatusCode.OK, "Email was successfully confirmed");
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to confirm the email.", 
                result
            );
        }

        [HttpPost("passwordRecovery")]
        [ValidateModel]
        public async Task<JsonResult> Post([FromBody]PasswordResetRequestModel model)
        {
            var user = _authService.FindUserByEmail(model.Email);
            string passwordResetToken = await _authService.GeneratePasswordResetTokenAsync(user);
            await _mailService.SendPasswordResetEmail(user.Email, Base64EncodingUtility.Encode(passwordResetToken));
            return MakeSuccessResponse(HttpStatusCode.OK, "Password reset email was sent.");
        }

        [HttpPost("{email}/passwordRecovery")]
        [ValidateModel]
        public async Task<JsonResult> Post([Required]string email, [FromBody]PasswordResetModel model)
        {
            var user = _authService.FindUserByEmail(email);
            var result = await _authService.ResetPasswordAsync(user, 
                Base64EncodingUtility.Decode(model.PasswordResetToken), 
                model.NewPassword);
                
            if (result.Succeeded)
            {
                return MakeSuccessResponse(HttpStatusCode.OK, "Password was reset. You can sign in now.");
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to reset the password.", 
                result
            );
        }


        [Authorize]
        [HttpPut]
        [ValidateModel]
        public async Task<JsonResult> Put([FromBody]AccountModel model)
        {
            var user = _authService.GetCurrentUser();
            bool emailChanged = user.Email != model.Email;
            
            if(emailChanged && !_userBusinessService.EmailAvailable(model.Email))
            {
                return MakeErrorResponse(HttpStatusCode.BadRequest, "Email is already taken.");
            }

            user.Email = model.Email;
            user.EmailConfirmed = false;
            var result = await _authService.UpdateAsync(user);
            
            if(result.Succeeded)
            {
                var responseMessage = "Account was updated.";
                if(emailChanged)
                {
                    string confirmationCode = await _authService.GenerateEmailConfirmationTokenAsync(user);
                    await _mailService.SendAccountConfirmationEmail(user.Email, Base64EncodingUtility.Encode(confirmationCode));
                    responseMessage = $@"The confirmation email was sent to {model.Email}.
                        Please, confirm it to be able to sign in with it next time.";
                }

                return MakeSuccessResponse(HttpStatusCode.OK, responseMessage);
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to update the account.", 
                result
            );
        }

        [Authorize]
        [HttpPut("passwordChange")]
        [ValidateModel]
        public async Task<JsonResult> Put([FromBody]PasswordChangeModel model)
        {
            var user = _authService.GetCurrentUser();
            var result = await _authService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            
            if(result.Succeeded)
            {
                return MakeSuccessResponse(HttpStatusCode.OK, "Password was successfully changed.");
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to change the password.", 
                result
            );
        }

        [Authorize]
        [HttpPost("passwordCreation")]
        [ValidateModel]
        public async Task<JsonResult> Post([FromBody]PasswordCreateModel model)
        {
            var user = _authService.GetCurrentUser();
            var result = await _authService.AddPasswordAsync(user, model.Password);

            if(result.Succeeded)
            {
                return MakeSuccessResponse(HttpStatusCode.OK, "Password was successfully created.");
            }

            return MakeIdentityErrorResponse(
                HttpStatusCode.InternalServerError, 
                "Failed to create the password.", 
                result
            );
        }

        private JsonResult MakeIdentityErrorResponse(HttpStatusCode status, string errorMessage, IdentityResult result)
        {
            string detailedErrorMessage = result.Errors.FirstOrDefault()?.Description ?? string.Empty;
            string fullErrorMessage = $"{errorMessage}. {detailedErrorMessage}";
            return MakeErrorResponse(HttpStatusCode.InternalServerError, fullErrorMessage);
        }
    }
}
