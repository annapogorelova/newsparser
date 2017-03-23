using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using newsparser.feedparser;
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;
using NewsParser.Identity;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsSourcesController: BaseController
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly IFeedUpdater _feedUpdater;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsSourcesController(INewsSourceBusinessService newsSourceBusinessService, 
            IFeedUpdater feedUpdater, 
            UserManager<ApplicationUser> userManager)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _feedUpdater = feedUpdater;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<JsonResult> Get(bool subscribed = false, string search = null, int pageIndex = 0, int pageSize = 5)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            int total;
            var newsSources = _newsSourceBusinessService
                .GetNewsSourcesPage(out total, pageIndex, pageSize, search, subscribed, user.GetId())
                .ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(new { data = newsSourcesModels, total });
        }

        [HttpGet("/newssources/{id}")]
        public JsonResult Get(int id)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(id);
            return new JsonResult(Mapper.Map<NewsSourceApiModel>(newsSource));
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]NewsSourceCreateModel newsSourceModel)
        {
            if (_newsSourceBusinessService.GetNewsSourceByUrl(newsSourceModel.RssUrl) != null)
            {
                return MakeResponse(HttpStatusCode.BadRequest, new { Message = "News source already exists" });
            }

            try
            {
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var addedNewsSource = await _feedUpdater.AddNewsSource(newsSourceModel.RssUrl, user.GetId());
                var addedNewsSourceModel = Mapper.Map<NewsSource, NewsSourceApiModel>(addedNewsSource);
                return MakeResponse(HttpStatusCode.Created, addedNewsSourceModel);
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, $"Failed to create a news source; {e.Message}");
            }
        }
    }
}
