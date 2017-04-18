﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.Auth;
using NewsParser.Helpers.Utilities;
using NewsParser.Services;

namespace NewsParser.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;

        public AccountController(IAuthService authService, IMailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
        }

        [Authorize]
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var user = _authService.GetCurrentUser();
                var userModel = Mapper.Map<AccountApiModel>(user);
                return new JsonResult(new { User = userModel });
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Failed to find the user");
            }
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]CreateAccountModel model)
        {
            if(!ModelState.IsValid)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Required data was not provided or was not in valid format.");
            }

            try
            {
                var result = await _authService.CreateAsync(model.Email, model.Password);
                
                if (result.Succeeded){
                    var user = _authService.FindUserByEmail(model.Email);
                    string confirmationCode = await _authService.GenerateEmailConfirmationTokenAsync(user);
                    await _mailService.SendAccountConfirmationEmail(user.Email, Base64EncodingUtility.Encode(confirmationCode));
                    return MakeResponse(HttpStatusCode.Created, "Account was created");
                }

                string detailedErrorMessage = result.Errors.FirstOrDefault()?.Description ?? string.Empty;
                string errorMessage = $"Failed to create the account. {detailedErrorMessage}";
                return MakeResponse(HttpStatusCode.InternalServerError, errorMessage);
            }
            catch (Exception)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Failed to create the account");
            }
        }

        [HttpPost("{email}/confirmation")]
        public async Task<JsonResult> Post(string email, [FromBody]AccountActivationModel model)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(model.ConfirmationToken))
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Required data is missing");
            }

            try
            {
                var user = _authService.FindUserByEmail(email);
                if (user == null)
                {
                    return MakeResponse(HttpStatusCode.BadRequest, "Account does not exist");
                }

                if(user.EmailConfirmed)
                {
                    return MakeResponse(HttpStatusCode.Forbidden, "Account has already been confirmed");
                }

                var result = await _authService.ConfirmEmail(user, Base64EncodingUtility.Decode(model.ConfirmationToken));
                if (result.Succeeded)
                {
                    return MakeResponse(HttpStatusCode.OK, "Account was activated.");
                }

                string detailedErrorMessage = result.Errors.FirstOrDefault()?.Description ?? string.Empty;
                string errorMessage = $"Failed to activate the account. {detailedErrorMessage}";
                return MakeResponse(HttpStatusCode.InternalServerError, errorMessage);
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Something went wrong.");
            }
        }

        [HttpPost("passwordRecovery")]
        public async Task<JsonResult> Post([FromBody]PasswordResetRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Email was not provided or was not in valid format.");
            }

            try
            {
                var user = _authService.FindUserByEmail(model.Email);
                if(user == null)
                {
                    return MakeResponse(HttpStatusCode.BadRequest, $"Account with email {model.Email} does not exist.");
                }

                string passwordResetToken = await _authService.GeneratePasswordResetTokenAsync(user);
                await _mailService.SendPasswordResetEmail(user.Email, Base64EncodingUtility.Encode(passwordResetToken));
                return MakeResponse(HttpStatusCode.OK, "Password reset email was sent.");
            }
            catch(Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Password reset request failed.");
            }
        }

        [HttpPost("{email}/passwordRecovery")]
        public async Task<JsonResult> Post(string email, [FromBody]PasswordResetModel model)
        {
            if (string.IsNullOrEmpty(email) || !ModelState.IsValid)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Required data is missing");
            }

            try
            {
                var user = _authService.FindUserByEmail(email);
                if (user == null)
                {
                    return MakeResponse(HttpStatusCode.BadRequest, "Account does not exist");
                }

                var result = await _authService.ResetPasswordAsync(user, 
                    Base64EncodingUtility.Decode(model.PasswordResetToken), 
                    model.NewPassword);

                if (result.Succeeded)
                {
                    return MakeResponse(HttpStatusCode.OK, "Password was reset. You can sign in now.");
                }

                string detailedErrorMessage = result.Errors.FirstOrDefault()?.Description ?? string.Empty;
                string errorMessage = $"Failed to reset the password. {detailedErrorMessage}";
                return MakeResponse(HttpStatusCode.InternalServerError, errorMessage);
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Something went wrong.");
            }
        }


        [Authorize]
        [HttpPut]
        public JsonResult Put([FromBody]AccountApiModel model)
        {
            // update account
            return MakeResponse(HttpStatusCode.OK, "Account was updated");
        }
    }
}
