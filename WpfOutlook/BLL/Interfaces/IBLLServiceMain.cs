using System.Collections.Generic;
using BLL.EntitesDTO;
using Model.Entities;

namespace BLL.Interfaces
{
    public interface IBLLServiceMain
    {
        IEnumerable<AppointmentDTO> GetAppointments();
        IEnumerable<AppointmentDTO> GetCalendar();
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(int id);
        IEnumerable<AppointmentDTO> GetCalendarByUserId(int id);
        ICollection<UserDTO> GetAppointmentUsers(int id);
        AppointmentDTO GetAppointmentById(int id);
        LocationDTO GetLocationById(int id);
        void AddAppointment(AppointmentDTO appointment, ICollection<UserDTO> usersDTO, int id);
        void RemoveAppointment(int id);
        void AddLocation(LocationDTO location);
        void AddUser(UserDTO user);
        void AddUser(User user);
        void AddLocation(Location location);
    }
}
