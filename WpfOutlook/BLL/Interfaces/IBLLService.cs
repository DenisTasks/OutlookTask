using BLL.EntitesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBLLService : IDisposable
    {
        IEnumerable<AppointmentDTO> GetAppointments();
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(AppointmentDTO appointment);
        AppointmentDTO GetAppointmentById(int id);
        LocationDTO GetLocationById(int id);
        UserDTO CheckUser(string name, string password);
        void AddAppointment(AppointmentDTO appointment);
        void RemoveAppointment(AppointmentDTO appointment);
        void AddLocation(LocationDTO location);
        void AddUser(UserDTO user);
    }
}
