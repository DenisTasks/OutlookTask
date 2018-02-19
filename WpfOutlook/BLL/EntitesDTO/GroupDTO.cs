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
        public virtual ICollection<UserDTO> Moderators { get; set; }

        public int CreatorId { get; set; }

        public virtual ICollection<UserDTO> Users { get; set; }
    }
}
