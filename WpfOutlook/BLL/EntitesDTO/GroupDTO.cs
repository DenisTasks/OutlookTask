using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntitesDTO
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public int ParentId { get; set; }

        public ICollection<GroupDTO> Childs { get; set; }

        public int CreatorId { get; set; }

        public ICollection<UserDTO> Users { get; set; }
        public ICollection<UserDTO> Moderators { get; set; }
    }
}
