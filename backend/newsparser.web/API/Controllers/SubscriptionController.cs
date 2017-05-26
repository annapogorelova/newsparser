using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsParser.API.Models;
using NewsParser.BL.Services.NewsSources;
using NewsParser.Helpers.ActionFilters.ModelValidation;
using NewsParser.Identity.Models;

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
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionController(INewsSourceBusinessService newsSourceBusinessService, 
            ILogger<SubscriptionController> log,
            UserManager<ApplicationUser> userManager)
        {
            _newsSourceBusinessService = newsSourceBusinessService;
            _log = log;
            _userManager = userManager;
        }

        [HttpPost("{id:int}")]
        [ValidateModel]
        public async Task<JsonResult> Post(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if(_newsSourceBusinessService.IsUserSubscribed(id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.Forbidden, "User is already subscribed to this news source");
            }
            _newsSourceBusinessService.AddNewsSourceToUser(id, user.GetId());
            return MakeSuccessResponse(HttpStatusCode.Created, "Successfully subscribed to news source");
        }

        [HttpDelete("{id:int}")]
        public async Task<JsonResult> Delete(int id)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if(!_newsSourceBusinessService.IsUserSubscribed(id, user.GetId()))
            {
                return MakeErrorResponse(HttpStatusCode.Forbidden, "User is not subscribed to this news source");
            }
            _newsSourceBusinessService.DeleteUserNewsSource(id, user.GetId());
            return MakeSuccessResponse(HttpStatusCode.OK, "Successfully unsubscribed from news source");
        }
    }
}
