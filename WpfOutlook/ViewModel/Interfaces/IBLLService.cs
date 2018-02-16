using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace ViewModel.Interfaces
{
    public interface IBLLService : IDisposable
    {
        IEnumerable<Appointment> GetAppointments();
        IEnumerable<Location> GetLocations();
        IEnumerable<User> GetUsers();
        void AddAppointment(Appointment appointment);
        void RemoveAppointment(Appointment appointment);
        void AddUser(User user);
    }
}
