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
using Microsoft.Extensions.Caching.Distributed;
using NewsParser.Cache;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly IAuthService _authService;
        private readonly IDistributedCache _distributedCache;

        public NewsController(
            INewsBusinessService newsBusinessService, 
            IAuthService authService, 
            IDistributedCache distributedCache)
        {
            _newsBusinessService = newsBusinessService;
            _authService = authService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [ValidateModel]
        [ResponseCache(Duration = 1800)]
        [Cache(Duration = 1800, DeferByUser = true)]
        public JsonResult Get(NewsListSelectModel model)
        {
            var user = _authService.GetCurrentUser();
            var news = _newsBusinessService.GetNewsPage
                (
                model.PageIndex,
                model.PageSize,
                user.GetId(),
                model.Search,
                model.Sources?.Select(int.Parse).Distinct().ToArray(),
                model.Tags.Distinct().ToArray()
                ).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(new { data = newsModels });
        }
    }
}
