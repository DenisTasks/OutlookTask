using Model.Entities;
using System.Data.Entity;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace Model
{
    public class WPFOutlookContext : DbContext
    {
        public WPFOutlookContext()
            : base("name=WPFOutlookContext")
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Role> Roles { get; set; }

    }
}
