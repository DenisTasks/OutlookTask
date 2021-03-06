﻿using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model;
using Model.Entities;
using Model.Interfaces;
using Model.ModelService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private WPFOutlookContext _context;
        private IGenericRepository<User> _users;

        public AuthenticationService()
        {
            _context = new WPFOutlookContext();
            _users = new GenericRepository<User>(_context);
        }

        public UserDTO AuthenticateUser(string username, string password)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            });
            IMapper mapper = config.CreateMapper();
            UserDTO user =mapper.Map<User, UserDTO>(_users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault());
            if (user != null)
                return user;
            else throw new UnauthorizedAccessException("Wrong credentials.");
            
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public string[] GetRoles(int userId)
        {
            return _users.FindById(userId).Roles.Select(r => r.Name).ToArray();
        }
    }
}
