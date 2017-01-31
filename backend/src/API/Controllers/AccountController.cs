using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.DAL.Users;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string userEmail = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _userRepository.GetUserByEmail(userEmail);
            var userModel = Mapper.Map<UserApiModel>(user);
            return new JsonResult(new { User = userModel });
        }
    }
}
