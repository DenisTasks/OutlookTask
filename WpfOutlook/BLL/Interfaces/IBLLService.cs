using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IBLLService : IDisposable
    {
        IEnumerable<AppointmentDTO> GetAppointments();
        IEnumerable<LocationDTO> GetLocations();
        IEnumerable<UserDTO> GetUsers();
        IEnumerable<AppointmentDTO> GetAppsByLocation(AppointmentDTO appointment);
        void AddAppointment(AppointmentDTO appointment);
        void RemoveAppointment(AppointmentDTO appointment);
        void AddLocation(LocationDTO location);
        void AddUser(UserDTO user);
    }
}
