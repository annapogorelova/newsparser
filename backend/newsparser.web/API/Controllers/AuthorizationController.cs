using System;
using System.Net;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using newsparser.DAL.Models;
using NewsParser.Auth;
using NewsParser.Auth.ExternalAuth;
using NewsParser.Exceptions;
using NewsParser.Identity.Models;

namespace NewsParser.API.Controllers
{
    /// <summary>
    /// Controller contains methods for authorizing users
    /// </summary>
    public class AuthorizationController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IExternalAuthService _externalAuthService;
        private readonly IAuthService _authService;

        public AuthorizationController(
            SignInManager<ApplicationUser> signInManager,
            IExternalAuthService externalAuthService,
            IAuthService authService)
        {
            _signInManager = signInManager;
            _externalAuthService = externalAuthService;
            _authService = authService;
        }

        [HttpPost("~/api/token"), Produces("application/json")]
        public async Task<IActionResult> Token(OpenIdConnectRequest request)
        {
            if (request.GrantType == "urn:ietf:params:oauth:grant-type:facebook_access_token")
            {
                return await HandleFacebookAuth(request);
            }

            if (request.GrantType == "urn:ietf:params:oauth:grant-type:google_access_token")
            {
                return await HandleGoogleAuth(request);
            }

            if (request.IsPasswordGrantType())
            {
                return await HandleTokenAuth(request);
            }

            if (request.IsRefreshTokenGrantType())
            {
                return await HandleRefreshToken(request);
            }

            throw new WebLayerException(HttpStatusCode.BadRequest, "The specified grant type is not supported.");
        }

        /// <summary>
        /// Handles username/password authentication
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Sign in action result</returns>
        private async Task<IActionResult> HandleTokenAuth(OpenIdConnectRequest request)
        {
            var user = _authService.FindUserByEmail(request.Username);
            if (user == null)
            {
                throw new WebLayerException(HttpStatusCode.BadRequest, "Invalid username or password");
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                throw new WebLayerException(HttpStatusCode.BadRequest, "The specified user cannot sign in.");
            }

            if (!_authService.CheckUserPassword(user, request.Password))
            {
                throw new WebLayerException(HttpStatusCode.BadRequest, "Invalid username or password");
            }

            var principal = await _authService.CreateUserPrincipalAsync(user);
            var ticket = _authService.CreateAuthTicket(request, principal);
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        /// <summary>
        /// Handles Facebook social authentication
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Sign in action result</returns>
        private async Task<IActionResult> HandleFacebookAuth(OpenIdConnectRequest request)
        {
            if (string.IsNullOrEmpty(request.Assertion))
            {
                return MakeResponse(HttpStatusCode.BadRequest, "The mandatory 'assertion' parameter was missing.");
            }

            var externalUser = await _externalAuthService.VerifyAccessTokenAsync(request.Assertion, ExternalAuthProvider.Facebook);
            
            if (!externalUser.IsVerified)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Facebook user is not verified.");
            }
            
            if(string.IsNullOrEmpty(externalUser.Email))
            {
                return MakeResponse(HttpStatusCode.BadRequest, @"Email is required. 
                    Please, make sure that you have it set on your facebook account.");
            }
            
            var user = _authService.FindUserByEmail(externalUser.Email);

            if (user == null)
            {
                user = await _authService.CreateExternalUserAsync(externalUser, ExternalAuthProvider.Facebook);
            }
            else
            {
                await _authService.UpdateExternalUserAsync(user, externalUser, ExternalAuthProvider.Facebook);
            }

            var principal = await _authService.CreateSocialUserPrincipalAsync(user, ExternalAuthProvider.Facebook);
            var ticket = _authService.CreateAuthTicket(request, principal);
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        /// <summary>
        /// Handles Google social authentication
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Sign in action result</returns>
        private async Task<IActionResult> HandleGoogleAuth(OpenIdConnectRequest request)
        {
            if (string.IsNullOrEmpty(request.Assertion))
            {
                return MakeResponse(HttpStatusCode.BadRequest, "The mandatory 'assertion' parameter was missing.");
            }

            var externalUser = await _externalAuthService.VerifyAccessTokenAsync(request.Assertion, ExternalAuthProvider.Google);
            if (!externalUser.IsVerified)
            {
                return MakeResponse(HttpStatusCode.BadRequest, "Facebook user is not verified.");
            }

            var user = _authService.FindUserByEmail(externalUser.Email);
            if (user == null)
            {
                user = await _authService.CreateExternalUserAsync(externalUser, ExternalAuthProvider.Google);
            }
            else
            {
                await _authService.UpdateExternalUserAsync(user, externalUser, ExternalAuthProvider.Google);
            }

            var principal = await _authService.CreateSocialUserPrincipalAsync(user, ExternalAuthProvider.Google);
            var ticket = _authService.CreateAuthTicket(request, principal);
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        private async Task<IActionResult> HandleRefreshToken(OpenIdConnectRequest request)
        {
                var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                var user = await _authService.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return MakeResponse(HttpStatusCode.BadRequest,"The refresh token is no longer valid.");
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                var ticket = _authService.CreateAuthTicket(request, info.Principal);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
    }
}

