using System.Collections.Generic;
using BLL.EntitesDTO;

namespace BLL.Interfaces
{
    public interface IBLLServiceMain
    {
        IEnumerable<AppointmentDTO> GetAppointments();
        IEnumerable<AppointmentDTO> GetAppointmentsByUserId(int id);
        IEnumerable<AppointmentDTO> GetCalendar();
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(int id);
        IEnumerable<AppointmentDTO> GetCalendarByUserId(int id);
        ICollection<UserDTO> GetAppointmentUsers(int id);
        AppointmentDTO GetAppointmentById(int id);
        LocationDTO GetLocationById(int id);
        void AddAppointment(AppointmentDTO appointment, ICollection<UserDTO> usersDTO, int id);
        void RemoveAppointment(int id, int userId);
    }
}
