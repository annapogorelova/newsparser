using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using NewsParser.Web.Auth.ExternalAuth;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Users;
using NewsParser.Exceptions;
using NewsParser.Web.Identity;
using NewsParser.Web.Identity.Models;
using OpenIddict.Core;

namespace NewsParser.Web.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                          OpenIdConnectConstants.Destinations.IdentityToken);
            }

            return principal;
        }

        public async Task<ClaimsPrincipal> CreateSocialUserPrincipalAsync(ApplicationUser user, ExternalAuthProvider authProvider)
        {
            var principal = await CreateUserPrincipalAsync(user);
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

        public AuthenticationTicket CreateAuthTicket(OpenIdConnectRequest request, ClaimsPrincipal principal)
        {
            var ticket = new AuthenticationTicket(
                    principal,
                    new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                }.Intersect(request.GetScopes()));

            return ticket;
        }

        public ApplicationUser GetCurrentUser()
        {
            var user = _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name).Result;
            CurrentUser.SetCurrentUser(user);
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
            user.EmailConfirmed = true;
            user.UserName = Guid.NewGuid().ToString();

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
            var existingUser = _userRepository.GetUserBySocialId(socialId, provider);
            return existingUser != null ? AutoMapper.Mapper.Map<User, ApplicationUser>(existingUser) : null;
        }

        public ApplicationUser FindUserByEmail(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
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

        public async Task<IdentityResult> CreateAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser != null)
            {
                throw new WebLayerException(HttpStatusCode.BadRequest, $"User with email {email} already exists.");
            }
            return _userManager.CreateAsync(new ApplicationUser { 
                Email = email, 
                UserName = Guid.NewGuid().ToString() 
            }, password).Result;
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public Task<IdentityResult> ConfirmEmail(ApplicationUser user, string confirmationToken)
        {
            return _userManager.ConfirmEmailAsync(user, confirmationToken);
        }

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string passwordResetToken, 
            string newPassword)
        {
            return _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return _userManager.UpdateAsync(user);
        }

        public ApplicationUser FindUserByUserName(string userName)
        {
            var user = _userRepository.GetUserByUserName(userName);
            return AutoMapper.Mapper.Map<User, ApplicationUser>(user);
        }

        public Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
        {
            return _userManager.AddPasswordAsync(user, password);
        }

        public Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return _userManager.GetUserAsync(principal);
        }
    }
}
