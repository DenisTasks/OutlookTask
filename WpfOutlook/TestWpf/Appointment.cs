using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpf
{
    public class Appointment
    {
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
    }
}
