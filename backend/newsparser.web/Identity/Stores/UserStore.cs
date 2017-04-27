using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using newsparser.DAL.Models;
using NewsParser.BL.Exceptions;
using NewsParser.BL.Services.Users;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Users;
using NewsParser.Identity.Models;

namespace NewsParser.Identity.Stores
{
    /// <summary>
    /// Class contains implementation of <see cref="IUserPasswordStore{TUser}"/>
    /// and <see cref="IUserEmailStore{TUser}"/> 
    /// using <see cref="IUserRepository"/>
    /// </summary>
    public class UserStore: IUserPasswordStore<ApplicationUser>, 
                            IUserEmailStore<ApplicationUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly Dictionary<string, ExternalAuthProvider> _authProvidersAliases;

        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToLower());
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName.ToLower();
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
                _userRepository.AddUser(Mapper.Map<ApplicationUser, User>(user));
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
                var existingUser = _userRepository.GetUserById(user.GetId());
                Mapper.Map(user, existingUser);
                _userRepository.UpdateUser(existingUser);
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

            var existingUser = _userRepository.GetUserById(user.GetId());

            try
            {
                _userRepository.DeleteUser(existingUser);
                return Task.FromResult(IdentityResult.Success);
            }
            catch (BusinessLayerException e)
            {
                throw new IdentityException("Failed to delete user", e);
            }
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetUserById(Convert.ToInt32(userId));
            return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            try
            {
                var user = _userRepository.GetUserByUserName(normalizedUserName.ToLower());
                return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
            }
            catch(EntityNotFoundException)
            {
                return null;
            }
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
            try
            {
                var user = _userRepository.GetUserByEmail(normalizedEmail.ToLower());
                return Task.FromResult(Mapper.Map<User, ApplicationUser>(user));
            }
            catch(EntityNotFoundException)
            {
                return null;
            }
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
    }
}
