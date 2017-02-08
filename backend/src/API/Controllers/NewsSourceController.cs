using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.DAL.NewsSources;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsSourceController: Controller
    {
        private readonly INewsSourceRepository _newsSourceRepository;

        public NewsSourceController(INewsSourceRepository newsSourceRepository)
        {
            _newsSourceRepository = newsSourceRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var newsSources = _newsSourceRepository.GetNewsSources().ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(newsSourcesModels);
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            var newsSource = _newsSourceRepository.GetNewsSourceById(id);
            return new JsonResult(Mapper.Map<NewsSourceApiModel>(newsSource));
        }
    }
}
