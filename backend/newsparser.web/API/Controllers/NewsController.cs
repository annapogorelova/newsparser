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
using newsparser.feedparser;
using NewsParser.BL.Services.NewsSources;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsController : BaseController
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly IFeedUpdater _feedUpdater;

        public NewsController(INewsBusinessService newsBusinessService, INewsSourceBusinessService newsSourceBusinessService, 
            IFeedUpdater feedUpdater)
        {
            _newsBusinessService = newsBusinessService;
            _newsSourceBusinessService = newsSourceBusinessService;
            _feedUpdater = feedUpdater;
        }

        [HttpGet]
        public async Task<JsonResult> Get(int? sourceId = null, int pageIndex = 0, int pageSize = 5, bool refresh = false)
        {
            //Hardcoded for now
            var userId = 2;
            if (refresh)
            {
                if (sourceId.HasValue)
                {
                    var newsSource = _newsSourceBusinessService.GetNewsSourceById(sourceId.Value);
                    if (newsSource.IsUpdating)
                    {
                        return MakeResponse(HttpStatusCode.Forbidden, "News source is already updating");
                    }
                }
                await _feedUpdater.UpdateFeedAsync(userId, sourceId);
            }
            var news = _newsBusinessService.GetNewsPage(pageIndex, pageSize, sourceId, userId).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(newsModels);
        }
    }
}
