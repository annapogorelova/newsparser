using System;
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
                    return MakeResponse(HttpStatusCode.OK, "Account was activated");
                }

                string detailedErrorMessage = result.Errors.FirstOrDefault()?.Description ?? string.Empty;
                string errorMessage = $"Failed to activate the account. {detailedErrorMessage}";
                return MakeResponse(HttpStatusCode.InternalServerError, errorMessage);
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Something went wrong");
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
