using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.BL.Services.News;
using NewsParser.DAL.Models;
using System.Linq;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly INewsBusinessService _newsBusinessService;

        public NewsController(INewsBusinessService newsBusinessService)
        {
            _newsBusinessService = newsBusinessService;
        }

        [HttpGet]
        public JsonResult Get(int? sourceId = null, int pageIndex = 0, int pageSize = 5)
        {
            //Hardcoded for now
            var userId = 2;
            var news = _newsBusinessService.GetNewsPage(pageIndex, pageSize, sourceId, userId).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(newsModels);
        }
    }
}
