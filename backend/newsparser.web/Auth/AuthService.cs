using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using newsparser.DAL.Models;
using NewsParser.Auth.ExternalAuth;
using NewsParser.BL.Services.Users;
using NewsParser.DAL.Models;
using NewsParser.Identity;
using NewsParser.Identity.Models;

namespace NewsParser.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserBusinessService _userBusinessService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserBusinessService userBusinessService,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _userBusinessService = userBusinessService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClaimsPrincipal> GetUserPrincipalAsync(ApplicationUser user)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                          OpenIdConnectConstants.Destinations.IdentityToken);
            }

            return principal;
        }

        public async Task<ClaimsPrincipal> GetSocialUserPrincipalAsync(ApplicationUser user, ExternalAuthProvider authProvider)
        {
            var principal = await GetUserPrincipalAsync(user);
            if (string.IsNullOrEmpty(principal.Identity.Name))
            {
                var userSocialId = user.ExternalIds.FirstOrDefault(s => s.AuthProvider == authProvider);
                if (userSocialId == null)
                {
                    throw new IdentityException($"User {user.Id} does not have social id from {authProvider} provider");
                }

                principal.Claims.Append(new Claim(ClaimTypes.Name, userSocialId.NormalizedExternalId)
                    .SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                              OpenIdConnectConstants.Destinations.IdentityToken));
            }

            return principal;
        }

        public AuthenticationTicket GetAuthTicket(ClaimsPrincipal principal)
        {
            var ticket = new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme);

            return ticket;
        }

        public ApplicationUser GetCurrentUser()
        {
            var user = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name).Result;
            return user;
        }

        public async Task<ApplicationUser> CreateExternalUserAsync(ExternalUser externalUser, ExternalAuthProvider authProvider)
        {
            var user = AutoMapper.Mapper.Map<ExternalUser, ApplicationUser>(externalUser);
            user.ExternalIds = new List<ExternalIdModel>
            {
                new ExternalIdModel
                {
                    ExternalId = externalUser.ExternalId,
                    AuthProvider = authProvider
                }
            };

            await _userStore.CreateAsync(user, CancellationToken.None);
            return FindUserByExternalId(externalUser.ExternalId, authProvider);
        }

        public async Task<ApplicationUser> UpdateExternalUserAsync(ApplicationUser applicationUser, ExternalUser externalUser, ExternalAuthProvider authProvider)
        {
            AutoMapper.Mapper.Map(externalUser, applicationUser);
            if (
                !applicationUser.ExternalIds.Any(
                    e => e.ExternalId == externalUser.ExternalId && e.AuthProvider == externalUser.AuthProvider))
            {
                applicationUser.ExternalIds.Add(new ExternalIdModel
                {
                    ExternalId = externalUser.ExternalId,
                    AuthProvider = authProvider
                });
            }

            await _userStore.UpdateAsync(applicationUser, CancellationToken.None);
            return FindUserByExternalId(externalUser.ExternalId, authProvider);
        }

        public ApplicationUser FindUserByExternalId(string socialId, ExternalAuthProvider provider)
        {
            var existingUser = _userBusinessService.GetUserBySocialId(socialId, provider);
            return existingUser != null ? AutoMapper.Mapper.Map<User, ApplicationUser>(existingUser) : null;
        }

        public ApplicationUser FindUserByEmail(string email)
        {
            var user = _userBusinessService.GetUserByEmail(email);
            return AutoMapper.Mapper.Map<User, ApplicationUser>(user);
        }

        public bool CheckUserPassword(ApplicationUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password).Result;
        }

        public ApplicationUser FindExternalUser(ExternalUser user)
        {
            return !string.IsNullOrEmpty(user.Email) ? 
                FindUserByEmail(user.Email) : 
                FindUserByExternalId(user.ExternalId, user.AuthProvider);
        }

        public Task<IdentityResult> CreateAsync(string email, string password)
        {
            return _userManager.CreateAsync(new ApplicationUser(){ Email = email, UserName = email }, password);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public Task<IdentityResult> ConfirmEmail(ApplicationUser user, string confirmationToken)
        {
            return _userManager.ConfirmEmailAsync(user, confirmationToken);
        }
    }
}
