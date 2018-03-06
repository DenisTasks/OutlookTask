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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany<Group>(s => s.Groups)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("GroupId");
                    cs.ToTable("UserGroups");
                });


            modelBuilder.Entity<User>()
                .HasMany<Appointment>(s => s.Appointments)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("AppointmentId");
                    cs.ToTable("UserAppointments");
                });

            //modelBuilder.Entity<Group>()
            //    .HasMany<Group>(s => s.Groups)
            //    .WithMany()
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("GroupId");
            //        cs.MapRightKey("RelatedId");
            //        cs.ToTable("GroupGroups");
            //    });


            //modelBuilder.Entity<Group>()
            //    .HasMany<Appointment>(s => s.Appointments)
            //    .WithMany(c => c.Users)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("UserId");
            //        cs.MapRightKey("AppointmentId");
            //        cs.ToTable("UserAppointments");
            //    });

        }
    }
}
