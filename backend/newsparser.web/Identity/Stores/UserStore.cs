using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using newsparser.DAL.Models;
using NewsParser.BL;
using NewsParser.BL.Services.Users;
using NewsParser.DAL.Models;
using NewsParser.Identity.Models;

namespace NewsParser.Identity.Stores
{
    /// <summary>
    /// Class contains implementation of <see cref="IUserPasswordStore{TUser}"/> 
    /// using <see cref="IUserBusinessService"/>
    /// </summary>
    public class UserStore: IUserPasswordStore<ApplicationUser>, 
                            IUserEmailStore<ApplicationUser>
    {
        private readonly IUserBusinessService _userBusinessService;
        private readonly Dictionary<string, ExternalAuthProvider> _authProvidersAliases;

        public UserStore(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
            _authProvidersAliases = new Dictionary<string, ExternalAuthProvider>()
            {
                {"facebook", ExternalAuthProvider.Facebook},
                {"google", ExternalAuthProvider.Google}
            };
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
            
            // if (!string.IsNullOrEmpty(user.Email))
            // {
            //     return Task.FromResult(user.Email);
            // }

            // var userSocialId = user.ExternalIds.FirstOrDefault();
            // if (userSocialId != null)
            // {
            //     return Task.FromResult(user.ExternalIds.FirstOrDefault().NormalizedExternalId);
            // }

            // throw new IdentityException("Failed to get unique username");
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.Email = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email.ToLower());
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Email = normalizedName.ToLower();
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                _userBusinessService.AddUser(Mapper.Map<ApplicationUser, User>(user));
                return Task.FromResult(IdentityResult.Success);
            }
            catch (BusinessLayerException e)
            {
                throw new IdentityException("Identity: failed to create user", e);
            }
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var existingUser = _userBusinessService.GetUserById(user.GetId());
                if (existingUser == null)
                {
                    throw new IdentityException($"User with id {user.GetId()} does not exist");
                }
                Mapper.Map(user, existingUser);
                _userBusinessService.UpdateUser(existingUser);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (BusinessLayerException e)
            {
                throw new IdentityException("Identity: failed to update user", e);
            }
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var existingUser = _userBusinessService.GetUserById(user.GetId());
            if (existingUser == null)
            {
                throw new ArgumentException($"User width id {user.Id} does not exist");
            }

            try
            {
                _userBusinessService.DeleteUser(existingUser.Id);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (BusinessLayerException e)
            {
                throw new IdentityException("Failed to delete user", e);
            }
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _userBusinessService.GetUserById(Convert.ToInt32(userId));
            if (user == null)
            {
                return null;
            }
            return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _userBusinessService.GetUserByEmail(normalizedUserName);
            return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));

            // var parsedSocialId = ParseSocialId(normalizedUserName);
            // user = _userBusinessService.GetUserBySocialId(parsedSocialId.Item1, parsedSocialId.Item2);
            // return user != null ? Task.FromResult(Mapper.Map<User, ApplicationUser>(user)) : null;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email.ToLower();
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return FindByNameAsync(normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email.ToLower());
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Email = normalizedEmail.ToLower();
            return Task.CompletedTask;
        }

        // private Tuple<string, ExternalAuthProvider> ParseSocialId(string socialId)
        // {
        //     string id = socialId.Split(':')[1];
        //     string providerAlias = socialId.Split(':')[0];
        //     ExternalAuthProvider provider = _authProvidersAliases[providerAlias.ToLower()];
        //     return new Tuple<string, ExternalAuthProvider>(id, provider);
        // }
    }
}
