using System;
using System.Collections.Generic;
using System.Linq;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Users;

namespace NewsParser.BL.Services.Users
{
    public class UserBusinessService: IUserBusinessService
    {
        private readonly IUserRepository _userRepository;

        public UserBusinessService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public IEnumerable<User> GetUsersByNewsSource(int newsSourceId)
        {
            return _userRepository.GetUsersByNewsSource(newsSourceId);
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            if (UserExists(user))
            {
                throw new BusinessLayerException("User already exists");
            }

            try
            {
                return _userRepository.AddUser(user);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed inserting user {user.Email}", e);
            }
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), $"User cannot be null");
            }

            if (_userRepository.GetUserById(user.Id) == null)
            {
                throw new BusinessLayerException($"User with id {user.Id} does not exist");
            }

            try
            {
                _userRepository.UpdateUser(user);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed updating user with id {user.Id}", e);
            }
        }

        public void DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                throw new BusinessLayerException($"User with id {id} does not exist");
            }

            try
            {
                _userRepository.DeleteUser(existingUser);
            }
            catch (Exception e)
            {
                throw new BusinessLayerException($"Failed deleting user with id {id}", e);
            }
        }

        public User GetUserBySocialId(string socialId, ExternalAuthProvider provider)
        {
            return _userRepository.GetUserBySocialId(socialId, provider);
        }

        public bool UserExists(User user)
        {
            User existingUser = null;

            if (!string.IsNullOrEmpty(user.Email))
            {
                existingUser = _userRepository.GetUserByEmail(user.Email);
            }
            else if (user.UserExternalIds.Any())
            {
                var userSocialId = user.UserExternalIds.FirstOrDefault();
                existingUser = _userRepository.GetUserBySocialId(userSocialId.ExternalId, userSocialId.AuthProvider);
            }

            return existingUser != null;
        }
    }
}
