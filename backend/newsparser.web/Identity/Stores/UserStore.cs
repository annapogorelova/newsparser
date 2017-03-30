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
    public class UserStore: IUserPasswordStore<ApplicationUser>
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
            if (!string.IsNullOrEmpty(user.Email))
            {
                return Task.FromResult(user.Email);
            }

            var userSocialId = user.ExternalIds.FirstOrDefault();
            if (userSocialId != null)
            {
                return Task.FromResult(user.ExternalIds.FirstOrDefault().NormalizedExternalId);
            }

            throw new IdentityException("Failed to get unique username");
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var existingUser = _userBusinessService.GetUserByEmail(user.Email);
            if (existingUser == null)
            {
                throw new ArgumentException("User does not exist", nameof(user));
            }

            if (_userBusinessService.GetUserByEmail(userName) != null)
            {
                throw new ArgumentException($"Email '{userName}' is already taken", nameof(userName));
            }

            try
            {
                existingUser.Email = userName;
                _userBusinessService.AddUser(existingUser);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (BusinessLayerException e)
            {
                throw new IdentityException("Identity: failed to create user", e);
            }
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return GetUserNameAsync(user, cancellationToken);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
                _userBusinessService.UpdateUser(Mapper.Map<ApplicationUser, User>(user));
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
            if (user != null)
            {
                return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
            }

            var parsedSocialId = ParseSocialId(normalizedUserName);
            user = _userBusinessService.GetUserBySocialId(parsedSocialId.Item1, parsedSocialId.Item2);
            return user != null ? Task.FromResult(Mapper.Map<User, ApplicationUser>(user)) : null;
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

        private Tuple<string, ExternalAuthProvider> ParseSocialId(string socialId)
        {
            string id = socialId.Split(':')[1];
            string providerAlias = socialId.Split(':')[0];
            ExternalAuthProvider provider = _authProvidersAliases[providerAlias.ToLower()];
            return new Tuple<string, ExternalAuthProvider>(id, provider);
        }
    }
}
