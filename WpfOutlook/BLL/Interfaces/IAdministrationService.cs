using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAdministrationService: IDisposable
    {
        void CreateUser(UserDTO user);
        bool CheckUser(string username);
        void AddUserToGroup(UserDTO user);
        void DeactivateUser(int id);
        void RemoveUserFromGroup(UserDTO user);
        void CreateGroup(GroupDTO group);
        void DeleteGroup(GroupDTO group);
        void CreateRole(RoleDTO role);
        void DeleteRole(RoleDTO role);
        void EditUser(UserDTO user);
        void ChangeUserStatus(UserDTO user);
        void ShowLogs();
        ICollection<RoleDTO> GetRoles();
        ICollection<GroupDTO> GetGroups();
    }
}
