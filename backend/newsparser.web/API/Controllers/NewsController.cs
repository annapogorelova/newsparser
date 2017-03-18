using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.News;
using NewsParser.DAL.Models;
using System.Linq;
using System.Net;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class NewsController : BaseController
    {
        private readonly INewsBusinessService _newsBusinessService;

        public NewsController(INewsBusinessService newsBusinessService)
        {
            _newsBusinessService = newsBusinessService;
        }

        [HttpGet]
        public JsonResult Get(NewsListSelectModel model)
        {
            if (!ModelState.IsValid)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Invalid request data");
            }

            //Hardcoded for now
            var userId = 2;
            var news = _newsBusinessService.GetNewsPage
                (
                model.PageIndex,
                model.PageSize,
                userId,
                model.Search,
                model.Sources?.Select(int.Parse).ToArray(),
                model.Tags
                ).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(new { data = newsModels });
        }
    }
}
