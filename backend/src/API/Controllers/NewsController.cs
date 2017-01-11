using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.DAL.Models;
using NewsParser.DAL.News;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public JsonResult Get(int? categoryId = null, int? sourceId = null, int startIndex = 0, int numResults = 5)
        {
            var news = _newsRepository.GetNews(startIndex, numResults, categoryId, sourceId).ToList();
            var newsModels = Mapper.Map<List<NewsItem>, List<NewsItemApiModel>>(news);
            return new JsonResult(newsModels);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var newsItem = _newsRepository.GetNewsById(id);
            return new JsonResult(newsItem);
        }
    }
}
