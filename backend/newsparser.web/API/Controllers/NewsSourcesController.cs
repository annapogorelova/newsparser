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
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsSourcesController: Controller
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly IFeedUpdater _feedUpdater;

        public NewsSourcesController(INewsSourceBusinessService newsSourceBusinessService, IFeedUpdater feedUpdater)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _feedUpdater = feedUpdater;
        }

        [HttpGet]
        public JsonResult Get(string search = null, int pageIndex = 0, int pageSize = 5)
        {
            // TODO: remove hadrcode
            var userId = 2;
            var newsSources = _newsSourceBusinessService.GetNewsSourcesPage(pageIndex, pageSize, search, userId).ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(newsSourcesModels);
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
                var userId = 2;
                var addedNewsSource = await _feedUpdater.AddNewsSource(newsSourceModel.RssUrl, userId);
                var addedNewsSourceModel = Mapper.Map<NewsSource, NewsSourceApiModel>(addedNewsSource);
                return MakeResponse(HttpStatusCode.Created, addedNewsSourceModel);
            }
            catch (Exception e)
            {
                return MakeResponse(HttpStatusCode.InternalServerError, $"Failed to create a news source; {e.Message}");
            }
        }

        private JsonResult MakeResponse(HttpStatusCode statusCode, object data)
        {
            Response.StatusCode = (int) statusCode;
            return new JsonResult(data);
        }

        private JsonResult MakeResponse(HttpStatusCode statusCode, string message)
        {
            Response.StatusCode = (int)statusCode;
            return new JsonResult( new { Message = message });
        }
    }
}
