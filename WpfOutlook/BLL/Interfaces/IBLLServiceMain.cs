using System.Collections.Generic;
using BLL.EntitesDTO;

namespace BLL.Interfaces
{
    public interface IBLLServiceMain
    {
        IEnumerable<AppointmentDTO> GetAppointmentsByUserId(int id);
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(int id);
        IEnumerable<AppointmentDTO> GetCalendarByUserId(int id);
        IEnumerable<UserDTO> GetAppointmentUsers(int id);
        LocationDTO GetLocationById(int id);
        void AddAppointment(AppointmentDTO appointment, int id);
        void RemoveAppointment(int id);
    }
}
