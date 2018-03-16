using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model.Entities;
using Model.Interfaces;

namespace BLL.BLLService
{
    public class BLLServiceMain : IBLLServiceMain
    {
        private readonly IGenericRepository<Appointment> _appointments;
        private readonly IGenericRepository<User> _users;
        private readonly IGenericRepository<Location> _locations;

        public BLLServiceMain(IGenericRepository<Appointment> appointments, IGenericRepository<User> users, IGenericRepository<Location> locations)
        {
            _appointments = appointments;
            _users = users;
            _locations = locations;
        }

        public IEnumerable<AppointmentDTO> GetAppointmentsByUserId(int id)
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get(x => x.Users.Any(s => s.UserId == id)).ToList();
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection).ToList();
            foreach (var item in mappingCollection)
            {
                item.Room = _locations.FindById(item.LocationId).Room;
            }
            return mappingCollection;
        }

        public IEnumerable<AppointmentDTO> GetCalendarByUserId(int id)
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get(x => x.Users.Any(s => s.UserId == id)).ToList();
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection).ToList();
            foreach (var item in mappingCollection)
            {
                item.Room = _locations.FindById(item.LocationId).Room;
            }
            return mappingCollection;
        }

        public LocationDTO GetLocationById(int id)
        {
            Location location;
            using (_locations.BeginTransaction())
            {
                location = _locations.FindById(id);
            }
            var mappingItem = Mapper.Map<Location, LocationDTO>(location);
            return mappingItem;
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var locationsMapper = Mapper.Map<IEnumerable<Location>, IEnumerable<LocationDTO>>(_locations.Get());
            return locationsMapper;
        }

        public IEnumerable<AppointmentDTO> GetAppsByLocation(int id)
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get(x => x.LocationId == id).ToList();
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection).ToList();
            foreach (var item in mappingCollection)
            {
                item.Room = _locations.FindById(item.LocationId).Room;
            }
            return mappingCollection;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(_users.Get());
        }

        public IEnumerable<UserDTO> GetAppointmentUsers(int id)
        {
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(_appointments.FindById(id).Users.ToList());
        }

        public void AddAppointment(AppointmentDTO appointment, int id)
        {
            var appointmentItem = Mapper.Map<AppointmentDTO, Appointment>(appointment);
            appointmentItem.OrganizerId = id;
            appointmentItem.Organizer = _users.FindById(id);
            appointmentItem.Location = _locations.FindById(appointmentItem.LocationId);
            appointmentItem.Users = new List<User>();
            var convert = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<User>>(appointment.Users);
            foreach (var item in convert)
            {
                if (_users.FindById(item.UserId) != null)
                {
                    appointmentItem.Users.Add(_users.FindById(item.UserId));
                }
            }

            using (var transaction = _appointments.BeginTransaction())
            {
                try
                {
                    _appointments.Create(appointmentItem);
                    _appointments.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e + " from BLL service!");
                }
            }
        }

        public void RemoveAppointment(int id)
        {
            using (var transaction = _appointments.BeginTransaction())
            {
                try
                {
                    var appointment = _appointments.FindById(id);
                    _appointments.Remove(appointment);
                    _appointments.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e + " from BLL");
                }
            }
        }
    }
}