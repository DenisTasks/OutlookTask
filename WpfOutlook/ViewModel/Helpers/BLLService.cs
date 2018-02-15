using System;
using System.Collections.Generic;
using System.Windows;
using Model.Entities;
using Model.Interfaces;
using ViewModel.Interfaces;

namespace ViewModel.Helpers
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
