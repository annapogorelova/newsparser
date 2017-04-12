using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.Auth;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var user = _authService.GetCurrentUser();
                var userModel = Mapper.Map<UserApiModel>(user);
                return new JsonResult(new { User = userModel });
            }
            catch 
            {
                return MakeResponse(HttpStatusCode.InternalServerError, "Failed to find the user");
            }
        }
    }
}
