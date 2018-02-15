using Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WPFOutlookContext : DbContext
    {
        public WPFOutlookContext()
            :base("name=WPFOutlookContext")
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
