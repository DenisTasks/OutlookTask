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
        UserDTO GetUserById(int id);
        ICollection<string> GetGroupAncestors(string groupName);
        bool CheckUser(string username);
        void DeactivateUser(int id);
        void CreateGroup(GroupDTO group);
        void DeleteGroup(GroupDTO group);
        void EditUser(UserDTO user);
        void ShowLogs();
        ICollection<RoleDTO> GetRoles();
        ICollection<GroupDTO> GetGroups();
        ICollection<UserDTO> GetUsers();
    }
}
