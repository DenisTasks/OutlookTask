﻿using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAdministrationService: IDisposable
    {
        void CreateUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles);
        void EditUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles);
        ICollection<RoleDTO> GetUserRoles(int id);
        ICollection<GroupDTO> GetUserGroups(int id);
        UserDTO GetUserById(int id);
        ICollection<string> GetGroupAncestors(string groupName);
        ICollection<string> GetGroupChildren(int id);
        bool CheckUser(string username);
        bool CheckGroup(string groupName);
        void DeactivateUser(int id);
        void CreateGroup(GroupDTO group, ICollection<GroupDTO> Groups, ICollection<UserDTO> users);
        void EditGroup(GroupDTO group, ICollection<GroupDTO> Groups, ICollection<UserDTO> users);
        void DeleteGroup(GroupDTO group);
        void ShowLogs();
        ICollection<GroupDTO> GetGroupGroups(int id);
        ICollection<UserDTO> GetGroupUsers(int id);
        ICollection<RoleDTO> GetRoles();
        ICollection<GroupDTO> GetGroups();
        ICollection<UserDTO> GetUsers();
    }
}
