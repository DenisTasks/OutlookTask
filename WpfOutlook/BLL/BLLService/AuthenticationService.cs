using AutoMapper;
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

        private IMapper userToUserDTO()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                .ForMember("UserId", opt => opt.MapFrom(s => s.UserId))
                .ForMember("IsActive", opt => opt.MapFrom(s => s.IsActive))
                .ForMember("Name", opt => opt.MapFrom(s => s.Name))
                .ForMember("UserName", opt => opt.MapFrom(s => s.UserName))
                .ForMember("Password", opt => opt.MapFrom(s => s.Password))
                .ForMember("Roles", opt => opt.MapFrom(s => s.Roles))
                .ForMember("Appointments", opt => opt.MapFrom(s => s.Appointments))
                .ForMember("Groups", opt => opt.MapFrom(s => s.Groups));
            }).CreateMapper();

            return mapper;
        }

        public AuthenticationService()
        {
            _context = new WPFOutlookContext();
            _users = new GenericRepository<User>(_context);
        }

        public UserDTO AuthenticateUser(string username, string password)
        {

            UserDTO user = userToUserDTO().Map<User, UserDTO>(_users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault());
            if (user != null)
                return user;
            else throw new UnauthorizedAccessException("Wrong credentials.");
            
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
