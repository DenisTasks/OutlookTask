using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
