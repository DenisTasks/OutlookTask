using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class User
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
