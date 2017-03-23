using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.News;
using NewsParser.DAL.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NewsParser.BL.Services.Users;
using NewsParser.Identity;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsController : BaseController
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsController(INewsBusinessService newsBusinessService, UserManager<ApplicationUser> userManager)
        {
            _newsBusinessService = newsBusinessService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<JsonResult> Get(NewsListSelectModel model)
        {
            if (!ModelState.IsValid)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Invalid request data");
            }
            
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var news = _newsBusinessService.GetNewsPage
                (
                model.PageIndex,
                model.PageSize,
                user.GetId(),
                model.Search,
                model.Sources?.Select(int.Parse).ToArray(),
                model.Tags
                ).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(new { data = newsModels });
        }
    }
}
