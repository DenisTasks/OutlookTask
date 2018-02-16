using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BLL.Interfaces;
using Model.Entities;
using Model.Interfaces;

namespace BLL
{
    public class BLLService : IBLLService
    {
        IUnitOfWork Database { get; set; }

        public BLLService(IUnitOfWork uOw)
        {
            Database = uOw;
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            return Database.Appointments.Get();
        }

        public IEnumerable<Location> GetLocations()
        {
            return Database.Locations.Get();
        }

        public IEnumerable<Appointment> GetAppsByLocation(Appointment appointment)
        {
            return Database.Appointments.Get(x => x.LocationId == appointment.LocationId);
        }

        public IEnumerable<User> GetUsers()
        {
            return Database.Users.Get();
        }

        public void AddAppointment(Appointment appointment)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Appointments.Create(appointment);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void AddLocation(Location location)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Locations.Create(location);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void AddUser(User user)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Users.Create(user);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void RemoveAppointment(Appointment appointment)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Appointments.Remove(appointment);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.ToString());
                }
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
