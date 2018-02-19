﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntitesDTO
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public string Room { get; set; }

        public ICollection<AppointmentDTO> Appointments { get; set; }
    }
}
