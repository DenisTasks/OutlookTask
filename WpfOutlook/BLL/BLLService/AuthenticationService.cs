using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model;
using Model.Entities;
using Model.Interfaces;
using System;
using System.Collections.Generic;
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
                return user;
            else throw new UnauthorizedAccessException("Wrong credentials.");  
        }
        
        public string[] GetRoles(int userId)
        {
            return _users.FindById(userId).Roles.Select(r => r.Name).ToArray();
        }
    }
}
