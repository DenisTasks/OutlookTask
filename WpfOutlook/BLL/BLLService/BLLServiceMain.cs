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
        private readonly IGenericRepository<Log> _logs;

        private IMapper GetDefaultMapper<TEntityFrom, TEntityTo>() where TEntityFrom : class  where  TEntityTo : class 
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TEntityFrom, TEntityTo>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
        private IMapper GetFromAppToAppDtoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Appointment, AppointmentDTO>()
                .ForSourceMember(d => d.Location, opt => opt.Ignore());
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }
        private ICollection<User> ConvertUsers(ICollection<UserDTO> usersDTO)
        {
            ICollection<User> users = new List<User>();
            var convert = GetDefaultMapper<UserDTO, User>().Map<IEnumerable<UserDTO>, IEnumerable<User>>(usersDTO);
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
            _logs = logs;
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
            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
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
            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
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
            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
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
            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            Appointment appointment;
            using (_appointments.BeginTransaction())
            {
                appointment = _appointments.FindById(id);
            }
            var mappingItem = GetFromAppToAppDtoMapper().Map<Appointment, AppointmentDTO>(appointment);
            return mappingItem;
        }

        public LocationDTO GetLocationById(int id)
        {
            Location location;
            using (_locations.BeginTransaction())
            {
                location = _locations.FindById(id);
            }
            var mappingItem = GetDefaultMapper<Location, LocationDTO>().Map<Location, LocationDTO>(location);
            return mappingItem;
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var locationsMapper =
                GetDefaultMapper<Location, LocationDTO>().Map<IEnumerable<Location>, IEnumerable<LocationDTO>>(_locations.Get());
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

            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return GetDefaultMapper<User, UserDTO>().Map<IEnumerable<User>, IEnumerable<UserDTO>>(_users.Get());
        }

        public ICollection<UserDTO> GetAppointmentUsers(int id)
        {
            return GetDefaultMapper<User, UserDTO>().Map<IEnumerable<User>, ICollection<UserDTO>>(_appointments.FindById(id).Users.ToList());
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
                    _logs.Create(new Log
                    {
                        AppointmentName = appointment.Subject,
                        ActionAuthorId = id,
                        CreatorId = id,
                        Action = "Add",
                        EventTime = DateTime.Now
                    });
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

        public void RemoveAppointment(int id, int userId)
        {
            using (var transaction = _appointments.BeginTransaction())
            {
                try
                {
                    var appointment = _appointments.FindById(id);
                    _logs.Create(new Log
                    {
                        AppointmentName = appointment.Subject,
                        ActionAuthorId = userId,
                        CreatorId = appointment.OrganizerId,
                        Action = "Remove",
                        EventTime = DateTime.Now
                    });
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