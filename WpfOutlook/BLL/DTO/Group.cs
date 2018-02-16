using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public ICollection<User> Moderators { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
