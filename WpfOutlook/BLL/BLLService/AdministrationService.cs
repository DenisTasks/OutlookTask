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

        private IMapper GetFromGroupDTOtoGroupMapper(ICollection<GroupDTO> groupsDTO, ICollection<UserDTO> usersDTO)
        {
            ICollection<Group> groups = new List<Group>();
            var convertGroups = GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, ICollection<Group>>(groupsDTO);
            foreach (var item in convertGroups)
            {
                if (_groups.FindById(item.GroupId) != null)
                {
                    groups.Add(_groups.FindById(item.GroupId));
                }
            }

            //ICollection<User> users = new List<User>();
            //var convertUsers = GetUserDTOToUserMapper().Map<IEnumerable<UserDTO>, ICollection<User>>(usersDTO);
            //foreach (var item in convertUsers)
            //{
            //    if (_users.FindById(item.UserId) != null)
            //    {
            //        users.Add(_users.FindById(item.UserId));
            //    }
            //}

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<GroupDTO, Group>()
            //        .ForMember(d => d.Users, opt => opt.MapFrom(s => users))
            //        .ForMember(d => d.Groups, opt => opt.MapFrom(s => groups));

            //});

            //IMapper mapper = config.CreateMapper();

            //return mapper;
            return null;
        }
        

        private ICollection<Group> ConvertGroupsDTO(ICollection<GroupDTO> groupsDTO)
        {
            ICollection<Group> groups = new List<Group>();
            var convert = GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, IEnumerable<Group>>(groupsDTO);
            foreach (var item in convert)
            {
                if (_groups.FindById(item.GroupId) != null)
                {
                    groups.Add(_groups.FindById(item.GroupId));
                }
            }
            return groups;
        }

        private ICollection<Role> ConvertRolesDTO(ICollection<RoleDTO> rolesDTO)
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

            return roles;
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
            //_groups.Create(GetFromGroupDTOtoGroupMapper(group.Groups, group.Users).Map<GroupDTO, Group>(group));
            //_context.SaveChanges();
        }


        public void CreateUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>()
                    .ForMember(d => d.Roles, opt => opt.MapFrom(s => ConvertRolesDTO(roles)))
                    .ForMember(d => d.Groups, opt => opt.MapFrom(s => ConvertGroupsDTO(groups)));

            });
            IMapper mapper = config.CreateMapper();
            _users.Create(mapper.Map<UserDTO, User>(user));
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
                //ICollection<Role> roles = new List<Role>();
                //var convertRoles = GetDefaultMapper<RoleDTO, Role>().Map<IEnumerable<RoleDTO>, IEnumerable<Role>>(user.Roles);
                //foreach (var item in convertRoles)
                //{
                //    if (_roles.FindById(item.RoleId) != null)
                //    {
                //        roles.Add(_roles.FindById(item.RoleId));
                //    }
                //}

                //if (user.Roles != userToEdit.Roles) userToEdit.Roles = roles;

                //ICollection<Group> groups = new List<Group>();
                //var convertGroups = GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, ICollection<Group>>(user.Groups);
                //foreach (var item in convertGroups)
                //{
                //    if (_groups.FindById(item.GroupId) != null)
                //    {
                //        groups.Add(_groups.FindById(item.GroupId));
                //    }
                //}
                //if (user.Groups != userToEdit.Groups) userToEdit.Groups = groups;
                //_users.Update(userToEdit);
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
            //return GetUserDTOToUserMapper().Map<User, UserDTO>(_users.Get(u => u.UserId == id).FirstOrDefault());
            return null;
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

                if (group != null && group.ParentId != null)
                {
                    groupName = _groups.FindById((int)group.ParentId).GroupName;
                }
                else { groupName = null; }
            }
            return ancstrorNameList;
        }
    }
}
