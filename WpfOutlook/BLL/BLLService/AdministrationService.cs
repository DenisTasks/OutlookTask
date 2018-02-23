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

            private IMapper GetUserDTOToUserMapper()
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

        public void CreateGroup(GroupDTO group)
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

        public void EditUser(UserDTO user)
        {
            if (user.UserName != null && user.Password != null)
            {
                User userToEdit = _users.FindById(user.UserId);
                if (user.Name != null) userToEdit.Name = user.Name;
                if ((user.UserName != null || userToEdit.UserName == user.UserName) && CheckUser(user.UserName)) userToEdit.UserName = user.UserName;
                if (user.Password != null) userToEdit.Password = user.Password;
                if (user.IsActive != userToEdit.IsActive) userToEdit.IsActive = user.IsActive;
                ICollection<Role> roles = new List<Role>();
                var convert = GetDefaultMapper<RoleDTO, Role>().Map<IEnumerable<RoleDTO>, IEnumerable<Role>>(user.Roles);
                foreach (var item in convert)
                {
                    if (_roles.FindById(item.RoleId) != null)
                    {
                        roles.Add(_roles.FindById(item.RoleId));
                    }
                }

                if (user.Roles != userToEdit.Roles) userToEdit.Roles = roles;
                if (user.Groups != userToEdit.Groups) userToEdit.Groups = GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, ICollection<Group>>(user.Groups);
                _users.Update(userToEdit);
                //userToEdit = GetFromUserDTOToUserMapper(user.Roles).Map<UserDTO, User>(user);
                //_users.Update(GetFromUserDTOToUserMapper(user.Roles).Map<UserDTO, User>(user));
                _context.SaveChanges();
            }
            
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

        public void DeactivateUser(int id)
        {
            User user = _users.FindById(id);
            if (user.IsActive == true)
                user.IsActive = false;
            else user.IsActive = true;
            _context.SaveChanges();
        }

        public UserDTO GetUserById(int id)
        {
            return GetUserDTOToUserMapper().Map<User, UserDTO>(_users.Get(u => u.UserId == id).FirstOrDefault());
        }

        public ICollection<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<User>, ICollection<UserDTO>>(_users.Get());
        }

        public ICollection<string> GetGroupAncestors(string groupName)
        {
            ICollection<string> ancstrorNameList = new List<string>();

            Group group;
            while (groupName != null)
            {
                ancstrorNameList.Add(groupName);
                group = _groups.Get(g => g.GroupName.Equals(groupName)).FirstOrDefault();
                if (group != null && group.ParentId!=null)
                {
                    groupName = _groups.FindById((int)group.ParentId).GroupName;
                }
                else { groupName = null; }
            }
            return ancstrorNameList;
        }
    }
}
