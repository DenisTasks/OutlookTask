using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Room { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
