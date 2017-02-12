using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using System.Linq;
using NewsParser.BL.NewsSources;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsSourceController: Controller
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;

        public NewsSourceController(INewsSourceBusinessService newsSourceBusinessService)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var newsSources = _newsSourceBusinessService.GetNewsSources().ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(newsSourcesModels);
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(id);
            return new JsonResult(Mapper.Map<NewsSourceApiModel>(newsSource));
        }
    }
}
