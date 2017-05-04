using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using newsparser.feedparser;
using NewsParser.Auth;
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;
using NewsParser.Helpers.ActionFilters.ModelValidation;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsSourcesController: BaseController
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly IAuthService _authService;
        private readonly IFeedUpdater _feedUpdater;

        public NewsSourcesController(
            INewsSourceBusinessService newsSourceBusinessService,
            IAuthService authService,
            IFeedUpdater feedUpdater)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _authService = authService;
            _feedUpdater = feedUpdater;
        }

        [HttpGet]
        public JsonResult Get(bool subscribed = false, string search = null, int pageIndex = 0, int pageSize = 5)
        {
            var user = _authService.GetCurrentUser();
            int total;
            var newsSources = _newsSourceBusinessService
                .GetNewsSourcesPage(out total, pageIndex, pageSize, search, subscribed, user.GetId())
                .ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(new { data = newsSourcesModels, total });
        }

        [HttpGet("{id:int}")]
        public JsonResult Get(int id)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(id);
            return new JsonResult(Mapper.Map<NewsSourceApiModel>(newsSource));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<JsonResult> Post([FromBody]NewsSourceCreateModel newsSourceModel)
        {
            if (_newsSourceBusinessService.GetNewsSourceByUrl(newsSourceModel.RssUrl) != null)
            {
                return MakeResponse(HttpStatusCode.BadRequest, new { message = "RSS source already exists" });
            }

            var user = _authService.FindUserByUserName(HttpContext.User.Identity.Name);
            var addedNewsSource = await _feedUpdater.AddNewsSource(newsSourceModel.RssUrl, user.GetId());
            var addedNewsSourceModel = Mapper.Map<NewsSource, NewsSourceApiModel>(addedNewsSource);
            return MakeResponse(HttpStatusCode.Created, 
                new { data = addedNewsSourceModel, 
                    message = "RSS source was added to the list of your subscriptions" });
        }
    }
}
