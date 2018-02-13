using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string Subject { get; set; }
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
