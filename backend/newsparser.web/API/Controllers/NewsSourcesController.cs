using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using System.Linq;
using NewsParser.BL.Services.NewsSources;

namespace NewsParser.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NewsSourcesController: Controller
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;

        public NewsSourcesController(INewsSourceBusinessService newsSourceBusinessService)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var userId = 2;
            var newsSources = _newsSourceBusinessService.GetUserNewsSources(userId).ToList();
            var newsSourcesModels = Mapper.Map<List<NewsSourceApiModel>>(newsSources);
            return new JsonResult(newsSourcesModels);
        }

        [HttpGet("/newssources/{id}")]
        public JsonResult Get(int id)
        {
            var newsSource = _newsSourceBusinessService.GetNewsSourceById(id);
            return new JsonResult(Mapper.Map<NewsSourceApiModel>(newsSource));
        }
    }
}
