using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }
        public string Subject { get; set; }
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        public int LocationId { get; set; }

        public ICollection<UserDTO> Users { get; set; }

    }
}
