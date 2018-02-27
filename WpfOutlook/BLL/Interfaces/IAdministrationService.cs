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
        void CreateUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles);
        void EditUser(UserDTO user, ICollection<GroupDTO> groups, ICollection<RoleDTO> roles);
        ICollection<RoleDTO> GetUserRoles(int id);
        ICollection<GroupDTO> GetUserGroups(int id);
        UserDTO GetUserById(int id);
        ICollection<string> GetGroupAncestors(string groupName);
        bool CheckUser(string username);
        void DeactivateUser(int id);
        void CreateGroup(GroupDTO group);
        void DeleteGroup(GroupDTO group);
        void ShowLogs();
        ICollection<RoleDTO> GetRoles();
        ICollection<GroupDTO> GetGroups();
        ICollection<UserDTO> GetUsers();
    }
}
