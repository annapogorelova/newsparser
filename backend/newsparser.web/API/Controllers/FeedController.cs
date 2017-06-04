using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.Feed;
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
    public class FeedController : BaseController
    {
        private readonly IFeedDataService _feedDataService;
        private readonly IAuthService _authService;
        private readonly IDistributedCache _distributedCache;

        public FeedController(
            IFeedDataService feedDataService, 
            IAuthService authService, 
            IDistributedCache distributedCache)
        {
            _feedDataService = feedDataService;
            _authService = authService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [ValidateModel]
        [ResponseCache(Duration = 1800)]
        [Cache(Duration = 1800, DeferByUser = true)]
        public JsonResult Get(FeedSelectModel model)
        {
            var user = _authService.GetCurrentUser();
            var news = _feedDataService.GetPage
                (
                model.PageIndex,
                model.PageSize,
                user.GetId(),
                model.Search,
                model.Channels?.Select(int.Parse).Distinct().ToArray(),
                model.Tags?.Distinct().ToArray()
                ).ToList();
            var feedModels = Mapper.Map<List<FeedItem>, List<FeedItemModel>>(news);
            return new JsonResult(new { data = feedModels });
        }
    }
}
