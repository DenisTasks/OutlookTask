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
    public class AdministrationService : IAdministrationService
    {
        private WPFOutlookContext _context;
        private IGenericRepository<User> _users;
        private IGenericRepository<Group> _groups;
        private IGenericRepository<Role> _roles;

        private IMapper GetDefaultMapper<TEntityFrom, TEntityTo>() where TEntityFrom : class where TEntityTo : class
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntityFrom, TEntityTo>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        private IMapper GetFromUserDTOToUserMapper(ICollection<RoleDTO> rolesDTO)
        {
            ICollection<Role> roles = new List<Role>();
            var convert = GetDefaultMapper<RoleDTO, Role>().Map<IEnumerable<RoleDTO>, IEnumerable<Role>>(rolesDTO);
            foreach (var item in convert)
            {
                if (_roles.FindById(item.RoleId) != null)
                {
                    roles.Add(_roles.FindById(item.RoleId));
                }
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>()
                    .ForMember(d => d.Roles, opt => opt.MapFrom(s => roles));
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }

            private IMapper UserDTOToUser()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>()
                .ForMember("IsActive", opt => opt.MapFrom(s => s.IsActive))
                .ForMember("Name", opt => opt.MapFrom(s => s.Name))
                .ForMember("UserName", opt => opt.MapFrom(s => s.UserName))
                .ForMember("Password", opt => opt.MapFrom(s => s.Password))
                .ForMember("Roles", opt => opt.MapFrom(s => GetDefaultMapper<RoleDTO, Role>().Map<IEnumerable<RoleDTO>, ICollection<Role>>(s.Roles)))
                .ForMember("Groups", opt => opt.MapFrom(s => GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, ICollection<Group>>(s.Groups)));
            }).CreateMapper();

            return mapper;
        }


        public AdministrationService()
        {
            _context = new WPFOutlookContext();
            _users = new GenericRepository<User>(_context);
            _roles = new GenericRepository<Role>(_context);
            _groups = new GenericRepository<Group>(_context);
        }

        public void AddUserToGroup(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void ChangeUserStatus(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void CreateGroup(GroupDTO group)
        {
            throw new NotImplementedException();
        }

        public void CreateRole(RoleDTO role)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserDTO user)
        {
            _users.Create(GetFromUserDTOToUserMapper(user.Roles).Map<UserDTO, User>(user));
            _context.SaveChanges();
        }

        public void DeleteGroup(GroupDTO group)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(RoleDTO role)
        {
            throw new NotImplementedException();
        }

        public void EditUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public ICollection<GroupDTO> GetGroups()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Group, GroupDTO>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<Group>, ICollection<GroupDTO>>(_groups.Get());
        }

        public ICollection<RoleDTO> GetRoles()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDTO>();
            }).CreateMapper();
            return mapper.Map <IEnumerable<Role>,ICollection<RoleDTO>> (_roles.Get());
        }

        public void RemoveUserFromGroup(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void ShowLogs()
        {
            throw new NotImplementedException();
        }

        public bool CheckUser(string username)
        {
            if (username == null || _users.Get(u => u.UserName.ToLower().Equals(username.ToLower())).Any())
                return false;
            else return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
