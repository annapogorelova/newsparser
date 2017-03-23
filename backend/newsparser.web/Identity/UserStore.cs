using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsParser.BL;
using NewsParser.BL.Services.Users;
using NewsParser.DAL.Models;

namespace NewsParser.Identity
{
    /// <summary>
    /// Class contains implementation of <see cref="IUserPasswordStore{TUser}"/> 
    /// using <see cref="IUserBusinessService"/>
    /// </summary>
    public class UserStore: IUserPasswordStore<ApplicationUser>
    {
        private readonly IUserBusinessService _userBusinessService;

        public UserStore(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
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
                throw new Exception("Identity: failed to create user", e);
            }
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
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
                throw new Exception("Identity: failed to create user", e);
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
                throw new Exception("Identity: failed to update user", e);
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
                throw new Exception("Failed to delete user", e);
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
            if (user == null)
            {
                return null;
            }
            return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
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
    }
}
