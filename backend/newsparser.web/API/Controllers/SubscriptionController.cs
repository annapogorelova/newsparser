using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsParser.API.Models;
using NewsParser.BL.Services.NewsSources;

namespace NewsParser.API.Controllers
{
    /// <summary>
    /// Controller for managing news sources subscriptions
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class SubscriptionController: BaseController
    {
        private readonly INewsSourceBusinessService _newsSourceBusinessService;
        private readonly ILogger<SubscriptionController> _log;

        public SubscriptionController(INewsSourceBusinessService newsSourceBusinessService, ILogger<SubscriptionController> log)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _log = log;
        }

        [HttpPost]
        public JsonResult Post([FromBody]CreateSubscriptionModel model)
        {
            var userId = 2;
            try
            {
                _newsSourceBusinessService.AddNewsSourceToUser(model.SourceId, userId);
                return MakeResponse(HttpStatusCode.Created, "Successfully subscribed to news source");
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                return MakeResponse(HttpStatusCode.InternalServerError, "Failed to subscribe to new source");
            }
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var userId = 2;
            try
            {
                _newsSourceBusinessService.DeleteUserNewsSource(id, userId);
                return MakeResponse(HttpStatusCode.OK, "Successfully unsubscribed from news source");
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                return MakeResponse(HttpStatusCode.InternalServerError, "Failed to unsubscribe from new source");
            }
        }
    }
}
