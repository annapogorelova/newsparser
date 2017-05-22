using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsParser.API.Models;
using NewsParser.Exceptions;

namespace NewsParser.API.Controllers
{
    /// <summary>
    /// Base controller class that contains common api controller methods
    /// </summary>
    public abstract class BaseController: Controller
    {
        /// <summary>
        /// Create a response with status and data object
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="data">Data object</param>
        /// <returns>JsonResult</returns>
        protected JsonResult MakeResponse(HttpStatusCode statusCode, object data)
        {
            Response.StatusCode = (int)statusCode;
            return new JsonResult(data);
        }

        /// <summary>
        /// Create a response with status and data object 
        /// </summary>
        /// <param name="statusCode">Status code</param>
        /// <param name="message">Message</param>
        /// <returns>JsonResult</returns>
        protected JsonResult MakeResponse(HttpStatusCode statusCode, string message)
        {
            throw new WebLayerException(statusCode, message);
        }
    }
}
