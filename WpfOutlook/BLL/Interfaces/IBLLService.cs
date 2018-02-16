using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace BLL.Interfaces
{
    public interface IBLLService : IDisposable
    {
        IEnumerable<Appointment> GetAppointments();
        IEnumerable<Location> GetLocations();
        IEnumerable<User> GetUsers();
        IEnumerable<Appointment> GetAppsByLocation(Appointment appointment);
        void AddAppointment(Appointment appointment);
        void RemoveAppointment(Appointment appointment);
        void AddLocation(Location location);
        void AddUser(User user);
    }
}
