using System.Collections.Generic;
using BLL.DTO;
using Model.Entities;

namespace BLL.Interfaces
{
    public interface IBLLService
    {
        IEnumerable<AppointmentDTO> GetAppointments();
        IEnumerable<AppointmentDTO> GetCalendar();
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(int id);
        IEnumerable<AppointmentDTO> GetCalendarByUserId(int id);
        AppointmentDTO GetAppointmentById(int id);
        LocationDTO GetLocationById(int id);
        void AddAppointment(AppointmentDTO appointment);
        void RemoveAppointment(AppointmentDTO appointment);
        void AddLocation(LocationDTO location);
        void AddUser(UserDTO user);
        void AddUser(User user);
        void AddLocation(Location location);
    }
}
