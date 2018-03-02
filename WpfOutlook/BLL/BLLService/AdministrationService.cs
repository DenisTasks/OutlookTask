using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model;
using Model.Entities;
using Model.Interfaces;
using Model.ModelService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.Services
{
    public class AdministrationService : IAdministrationService
    {
        private WPFOutlookContext _context;
        private IGenericRepository<User> _users;
        private IGenericRepository<Group> _groups;
        private IGenericRepository<Role> _roles;

        #region mappers
        private IMapper GetDefaultMapper<TEntityFrom, TEntityTo>() where TEntityFrom : class where TEntityTo : class
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntityFrom, TEntityTo>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        private ICollection<Group> ConvertGroupsDTO(ICollection<GroupDTO> groupsDTO)
        {
            ICollection<Group> groups = new List<Group>();
            if (groupsDTO != null)
            {
                var convert = GetDefaultMapper<GroupDTO, Group>().Map<IEnumerable<GroupDTO>, IEnumerable<Group>>(groupsDTO);
                foreach (var item in convert)
                {
                    if (_groups.FindById(item.GroupId) != null)
                    {
                        groups.Add(_groups.FindById(item.GroupId));
                    }
                }
            }
            return groups;
        }

        private ICollection<Role> ConvertRolesDTO(ICollection<RoleDTO> rolesDTO)
        {
            ICollection<Role> roles = new List<Role>();
            if (rolesDTO != null)
            {
                var convert = GetDefaultMapper<RoleDTO, Role>().Map<IEnumerable<RoleDTO>, IEnumerable<Role>>(rolesDTO);
                foreach (var item in convert)
                {
                    if (_roles.FindById(item.RoleId) != null)
                    {
                        roles.Add(_roles.FindById(item.RoleId));
                    }
                }
            }

            return roles;
        }

        private ICollection<User> ConvertUsersDTO(ICollection<UserDTO> usersDTO)
        {
            ICollection<User> users = new List<User>();
            if (usersDTO != null)
            {
                var convert = GetDefaultMapper<UserDTO, User>().Map<IEnumerable<UserDTO>, ICollection<User>>(usersDTO);
                foreach (var item in convert)
                {
                    if (_users.FindById(item.UserId) != null)
                    {
                        users.Add(_users.FindById(item.UserId));
                    }
                }
            }
            return users;
        }

        #endregion
        
        public AdministrationService(IGenericRepository<User> users, IGenericRepository<Role> roles, IGenericRepository<Group> groups, WPFOutlookContext context)
        {
            _context = context;
            _users = users;
            _roles = roles;
            _groups = groups;
        }

        #region GroupMethods

        public void CreateGroup(GroupDTO groupDTO, ICollection<GroupDTO> groups, ICollection<UserDTO> users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupDTO, Group>()
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => ConvertUsersDTO(users)));
            });
            IMapper mapper = config.CreateMapper();
            Group group = mapper.Map<GroupDTO, Group>(groupDTO);
            _groups.Create(group);
            _context.SaveChanges();
            foreach (var item in ConvertGroupsDTO(groups))
            {
                item.ParentId = group.GroupId;
            }
            _context.SaveChanges();
        }

        public void DeleteGroup(GroupDTO groupDTO)
        {
            Group group = _groups.FindById(groupDTO.GroupId);
            ICollection<Group> childs = _groups.Get(g => g.ParentId == groupDTO.GroupId).ToList();
            if (groupDTO.ParentId != null)
            {
                foreach (var item in childs)
                {
                    Group child = _groups.FindById(item.GroupId);
                    child.ParentId = groupDTO.ParentId;
                    _groups.Update(child);
                }
            }
            else
            {
                foreach (var item in childs)
                {
                    Group child = _groups.FindById(item.GroupId);
                    child.ParentId = null;
                    _groups.Update(child);
                }
            }
            _groups.Remove(group);
            _context.SaveChanges();
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
                    groupName = _groups.Get(g => g.GroupId == group.ParentId).FirstOrDefault().GroupName;
                }
                else { groupName = null; }
            }
            return ancstrorNameList;
        }

        public ICollection<string> GetGroupChildren(int id)
        {
            SqlParameter param = new SqlParameter("@groupId", id);
            var groups = _context.Database.SqlQuery<string>("GetGroupChilds @groupId", param).ToList();

            //for graph
            /*var groupsCollection = _groups.Get(g => g.GroupId == id).FirstOrDefault().Groups;
            if (groupsCollection.Any())
            {
                foreach (var item in groupsCollection)
                {
                    groups.Add(item.GroupName);
                    groups.AddRange(GetGroupChildren(item.GroupId));
                }
            }*/

            return groups.Distinct().ToList();
        }

        public bool CheckGroup(string groupName)
        {
            if (_groups.Get(g => g.GroupName.ToLower().Equals(groupName.ToLower())).Any())
                return false;
            else return true;
        }

        public ICollection<GroupDTO> GetGroupFirstGeneration(int id)
        {
            return GetDefaultMapper<Group, GroupDTO>().Map<IEnumerable<Group>, ICollection<GroupDTO>>(_groups.Get(g => g.ParentId == id));
        }

        public void EditGroup(GroupDTO group, ICollection<GroupDTO> selectedGroups, ICollection<UserDTO> users)
        {
            if (group.GroupName != null)
            {
                Group groupToEdit = _groups.FindById(group.GroupId);
                if (group.GroupName != null && group.GroupName != groupToEdit.GroupName) groupToEdit.GroupName = group.GroupName;
                if (group.ParentId != groupToEdit.ParentId) groupToEdit.ParentId = group.ParentId;
                if (users.Any()) groupToEdit.Users = ConvertUsersDTO(users);
                if (!users.Any()) groupToEdit.Users = null;
                ICollection<Group> oldGroups = _groups.Get(g => g.ParentId == groupToEdit.GroupId).ToList();
                foreach (var item in oldGroups.ToList())
                {
                    foreach (var temp in selectedGroups.ToList())
                    {
                        if (item.GroupId == temp.GroupId)
                        {
                            oldGroups.Remove(item);
                            selectedGroups.Remove(temp);
                        }
                    }
                }
                if (oldGroups.Any())
                {
                    foreach (var item in oldGroups)
                    {
                        var temp = _groups.FindById(item.GroupId);
                        temp.ParentId = null;
                    }
                }
                if (selectedGroups.Any())
                {
                    foreach (var item in ConvertGroupsDTO(selectedGroups))
                    {
                        item.ParentId = groupToEdit.GroupId;
                    }
                }
                _context.SaveChanges();
            }
        }

        public ICollection<GroupDTO> GetGroupsWithNoAncestors()
        {
            return GetDefaultMapper<Group, GroupDTO>().Map<IEnumerable<Group>, ICollection<GroupDTO>>(_groups.Get(g => g.ParentId == null));
        }

        public ICollection<UserDTO> GetGroupUsers(int id)
        {
            return GetDefaultMapper<User, UserDTO>().Map<IEnumerable<User>, ICollection<UserDTO>>(_groups.FindById(id).Users);
        }

        #endregion
        
        #region UserMethods

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
        
        public void EditUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles)
        {
            if (user.UserName != null && user.Password != null)
            {
                User userToEdit = _users.FindById(user.UserId);
                if (user.Name != null) userToEdit.Name = user.Name;
                if ((user.UserName != null || userToEdit.UserName == user.UserName) && CheckUser(user.UserName)) userToEdit.UserName = user.UserName;
                if (user.Password != null) userToEdit.Password = user.Password;
                if (user.IsActive != userToEdit.IsActive) userToEdit.IsActive = user.IsActive;
                if (roles.Any()) userToEdit.Roles = ConvertRolesDTO(roles);
                if (!roles.Any()) userToEdit.Roles = null;
                if (groups.Any()) userToEdit.Groups = ConvertGroupsDTO(groups);
                if (!groups.Any()) userToEdit.Groups = null;
                _users.Update(userToEdit);
                _context.SaveChanges();
            }
        }
        
        public bool CheckUser(string username)
        {
            if (username == null || _users.Get(u => u.UserName.ToLower().Equals(username.ToLower())).Any())
                return false;
            else return true;
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
            return GetDefaultMapper<User, UserDTO>().Map<User, UserDTO>(_users.Get(u => u.UserId == id).FirstOrDefault());
        }

        public ICollection<RoleDTO> GetUserRoles(int id)
        {
            return GetDefaultMapper<Role, RoleDTO>().Map<IEnumerable<Role>,ICollection<RoleDTO>>(_users.FindById(id).Roles);
        }

        public ICollection<GroupDTO> GetUserGroups(int id)
        {
            return GetDefaultMapper<Group, GroupDTO>().Map<IEnumerable<Group>, ICollection<GroupDTO>>(_users.FindById(id).Groups);
        }

        #endregion
        
        public ICollection<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<User>, ICollection<UserDTO>>(_users.Get());
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
            return mapper.Map<IEnumerable<Role>, ICollection<RoleDTO>>(_roles.Get());
        }

        public void ShowLogs()
        {
            throw new NotImplementedException();
        }
    }
}
