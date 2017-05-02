using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.News;
using NewsParser.DAL.Models;
using System.Linq;
using System.Net;
using NewsParser.Auth;
using NewsParser.Helpers.ActionFilters.ModelValidation;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsController : BaseController
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly IAuthService _authService;

        public NewsController(INewsBusinessService newsBusinessService, IAuthService authService)
        {
            _newsBusinessService = newsBusinessService;
            _authService = authService;
        }

        [HttpGet]
        [ValidateModel]
        public JsonResult Get(NewsListSelectModel model)
        {
            var user = _authService.GetCurrentUser();
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
