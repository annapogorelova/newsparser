using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.Users;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserBusinessService _userBusinessService;

        public AccountController(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string userEmail = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userBusinessService.GetUserByEmail(userEmail);
            var userModel = Mapper.Map<UserApiModel>(user);
            return new JsonResult(new { User = userModel });
        }
    }
}
