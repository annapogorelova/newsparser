using System;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using newsparser.DAL.Models;
using NewsParser.Auth;
using NewsParser.Auth.ExternalAuth;
using NewsParser.Identity.Models;

namespace NewsParser.API.Controllers
{
    /// <summary>
    /// Controller contains methods for authorizing users
    /// </summary>
    public class AuthorizationController : Controller
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
        public IActionResult Token(OpenIdConnectRequest request)
        {
            if (request.GrantType == "urn:ietf:params:oauth:grant-type:facebook_access_token")
            {
                return HandleFacebookAuth(request).Result;
            }

            if (request.GrantType == "urn:ietf:params:oauth:grant-type:google_access_token")
            {
                return HandleGoogleAuth(request).Result;
            }

            if (request.IsPasswordGrantType())
            {
                return HandleTokenAuth(request).Result;
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
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
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Invalid username or password"
                });
            }

            //?
            if (!await _signInManager.CanSignInAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The specified user cannot sign in."
                });
            }

            if (!_authService.CheckUserPassword(user, request.Password))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Invalid username or password"
                });
            }

            var principal = await _authService.GetUserPrincipalAsync(user);
            var ticket = _authService.GetAuthTicket(principal);
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
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidRequest,
                    ErrorDescription = "The mandatory 'assertion' parameter was missing."
                });
            }

            try
            {
                var externalUser = await _externalAuthService.VerifyAccessTokenAsync(request.Assertion, ExternalAuthProvider.Facebook);
                if (!externalUser.IsVerified)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidRequest,
                        ErrorDescription = "Facebook user is not verified."
                    });
                }

                var user = _authService.FindExternalUser(externalUser);

                if (user == null)
                {
                    user = await _authService.CreateExternalUserAsync(externalUser, ExternalAuthProvider.Facebook);
                }
                else
                {
                    await _authService.UpdateExternalUserAsync(user, externalUser, ExternalAuthProvider.Facebook);
                }

                var principal = await _authService.GetSocialUserPrincipalAsync(user, ExternalAuthProvider.Facebook);
                var ticket = _authService.GetAuthTicket(principal);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            catch (Exception e)
            {
                return StatusCode(500, new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "Failed to authenticate Facebook user."
                });
            }
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
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidRequest,
                    ErrorDescription = "The mandatory 'assertion' parameter was missing."
                });
            }

            try
            {
                var externalUser = await _externalAuthService.VerifyAccessTokenAsync(request.Assertion, ExternalAuthProvider.Google);
                if (!externalUser.IsVerified)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidRequest,
                        ErrorDescription = "Facebook user is not verified."
                    });
                }

                var user = _authService.FindExternalUser(externalUser);

                if (user == null)
                {
                    user = await _authService.CreateExternalUserAsync(externalUser, ExternalAuthProvider.Google);
                }
                else
                {
                    await _authService.UpdateExternalUserAsync(user, externalUser, ExternalAuthProvider.Google);
                }

                var principal = await _authService.GetSocialUserPrincipalAsync(user, ExternalAuthProvider.Google);
                var ticket = _authService.GetAuthTicket(principal);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            catch (Exception e)
            {
                return StatusCode(500, new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.ServerError,
                    ErrorDescription = "Failed to authenticate Facebook user."
                });
            }
        }
    }
}

