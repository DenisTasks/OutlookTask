using System;
using System.Collections.Generic;
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

        private IEnumerable<User> ConvertUsers(IEnumerable<UserDTO> usersDTO)
        {
            ICollection<User> users = new List<User>();
            var convert = Mapper.Map<IEnumerable<UserDTO>, IEnumerable<User>>(usersDTO);
            foreach (var item in convert)
            {
                if (_users.FindById(item.UserId) != null)
                {
                    users.Add(_users.FindById(item.UserId));
                }
            }
            return users;
        }

        public BLLServiceMain(IGenericRepository<Appointment> appointments, IGenericRepository<User> users, IGenericRepository<Location> locations, IGenericRepository<Log> logs)
        {
            _appointments = appointments;
            _users = users;
            _locations = locations;
        }

        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get().ToList();
            }
            foreach (var item in collection)
            {
                item.Location = _locations.FindById(item.LocationId);
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public IEnumerable<AppointmentDTO> GetAppointmentsByUserId(int id)
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get(x => x.Users.Any(s => s.UserId == id)).ToList();
            }
            foreach (var item in collection)
            {
                item.Location = _locations.FindById(item.LocationId);
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public IEnumerable<AppointmentDTO> GetCalendar()
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get().ToList();
            }
            foreach (var item in collection)
            {
                item.Location = _locations.FindById(item.LocationId);
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public IEnumerable<AppointmentDTO> GetCalendarByUserId(int id)
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                collection = _appointments.Get(x => x.Users.Any(s => s.UserId == id)).ToList();
            }
            foreach (var item in collection)
            {
                item.Location = _locations.FindById(item.LocationId);
            }
            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            Appointment appointment;
            using (_appointments.BeginTransaction())
            {
                appointment = _appointments.FindById(id);
            }
            var mappingItem = Mapper.Map<Appointment, AppointmentDTO>(appointment);
            return mappingItem;
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
            foreach (var item in collection)
            {
                item.Location = _locations.FindById(item.LocationId);
            }

            var mappingCollection = Mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
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

        public void AddAppointment(AppointmentDTO appointment, ICollection<UserDTO> usersDTO , int id)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>()
                    .ForMember(d=> d.OrganizerId , opt => opt.MapFrom(s=> id))
                    .ForMember(d=> d.Organizer, opt=> opt.MapFrom(s=> _users.FindById(id)))
                    .ForMember(s => s.Location, opt => opt.MapFrom(loc => _locations.FindById(loc.LocationId)))
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => ConvertUsers(usersDTO)));
            }).CreateMapper();
            var appointmentItem = mapper.Map<AppointmentDTO, Appointment>(appointment);
            
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