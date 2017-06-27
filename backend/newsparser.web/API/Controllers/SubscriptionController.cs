using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsParser.API.Models;
using NewsParser.BL.Services.Channels;
using NewsParser.Helpers.ActionFilters.ModelValidation;
using NewsParser.Web.Identity.Models;

namespace NewsParser.API.Controllers
{
    /// <summary>
    /// Controller for managing channels subscriptions
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class SubscriptionController: BaseController
    {
        private readonly IChannelDataService _channelDataService;
        private readonly ILogger<SubscriptionController> _log;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionController(IChannelDataService channelDataService, 
            ILogger<SubscriptionController> log,
            UserManager<ApplicationUser> userManager)
        {
            _channelDataService = channelDataService;
            _log = log;
            _userManager = userManager;
        }

        [HttpPost("{id:int}")]
        [ValidateModel]
        public async Task<JsonResult> Post(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if(!_channelDataService.IsVisibleToUser(id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.NotFound, "Channel was not found.");
            }
            if(_channelDataService.IsUserSubscribed(id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.BadRequest, "User is already subscribed to this channel.");
            }

            _channelDataService.SubscribeUser(id, user.GetId());
            return MakeSuccessResponse(HttpStatusCode.Created, "Successfully subscribed to channel.");
        }

        [HttpDelete("{id:int}")]
        public async Task<JsonResult> Delete(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var channel = _channelDataService.GetById(id);
            if(!_channelDataService.IsVisibleToUser(channel.Id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.NotFound, "Channel was not found.");
            }
            if(!_channelDataService.IsUserSubscribed(channel.Id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.BadRequest, "User is not subscribed to this channel.");
            }
            _channelDataService.UnsubscribeUser(channel.Id, user.GetId());
            return MakeSuccessResponse(HttpStatusCode.OK, "Successfully unsubscribed from channel.");
        }
    }
}
