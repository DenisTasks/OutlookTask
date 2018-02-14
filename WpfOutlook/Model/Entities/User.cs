﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class User 
    {
        [Key]
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }

        public bool IsAuthenticated { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        
        public ICollection<Role> Roles { get; set; }

       
    }
}