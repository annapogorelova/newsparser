using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.News;
using NewsParser.DAL.Models;
using System.Linq;
using System.Threading.Tasks;
using newsparser.feedparser;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsController : Controller
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly IFeedUpdater _feedUpdater;

        public NewsController(INewsBusinessService newsBusinessService, IFeedUpdater feedUpdater)
        {
            _newsBusinessService = newsBusinessService;
            _feedUpdater = feedUpdater;
        }

        [HttpGet]
        public async Task<JsonResult> Get(int? sourceId = null, int pageIndex = 0, int pageSize = 5, bool refresh = false)
        {
            //Hardcoded for now
            var userId = 2;
            if (refresh)
            {
                await _feedUpdater.UpdateFeedAsync(userId, sourceId);
            }
            var news = _newsBusinessService.GetNewsPage(pageIndex, pageSize, sourceId, userId).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(newsModels);
        }
    }
}
