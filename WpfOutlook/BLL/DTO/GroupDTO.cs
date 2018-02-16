using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public ICollection<UserDTO> Moderators { get; set; }

        public int CreatorId { get; set; }
        public UserDTO Creator { get; set; }

        public ICollection<UserDTO> Users { get; set; }
    }
}
