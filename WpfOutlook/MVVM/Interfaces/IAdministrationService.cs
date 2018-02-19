using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Interfaces
{
    public interface IAdministrationService
    {
        void AddUserToGroup(UserDTO user);
        void RemoveUserFromGroup(UserDTO user);
        void CreateGroup(GroupDTO group);
        void DeleteGroup(GroupDTO group);
        void CreateRole(RoleDTO role);
        void DeleteRole(RoleDTO role);
        void EditUser(UserDTO user);
        void ChangeUserStatus(UserDTO user);
        void ShowLogs();
    }
}
