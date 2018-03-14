using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model.Entities;
using Model.Helpers;
using Model.Interfaces;
using System;
using System.Linq;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IGenericRepository<User> _users;

        public AuthenticationService(IGenericRepository<User> users)
        {
            _users = users;
        }

        public UserDTO AuthenticateUser(string username, string password)
        {
            UserDTO user = Mapper.Map<User, UserDTO>(_users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault());
            if (user != null && user.IsActive)
            {
                using (var transaction = _users.BeginTransaction())
                {
                    user = _users.FindById(user.UserId);
                    user.Salt = EncryptionHelpers.GenerateSalt();
                    user.Password = EncryptionHelpers.HashPassword(username, password, user.Salt);
                    _users.Save();
                    transaction.Commit();
                }
                return mapper.Map<User,UserDTO>(user);
            }
            else throw new UnauthorizedAccessException("Wrong credentials.");  
        }
        
        public string[] GetRoles(int userId)
        {
            return _users.FindById(userId).Roles.Select(r => r.Name).ToArray();
        }
        
    }
}
