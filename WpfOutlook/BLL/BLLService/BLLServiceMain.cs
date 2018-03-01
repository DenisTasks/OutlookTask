using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AutoMapper;
using BLL.DTO;
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
                .ForMember("Room", opt => opt.MapFrom(s => s.Location.Room))
                .ForSourceMember(d => d.Location, opt => opt.Ignore())
                .ForMember(d => d.Users, opt => opt.MapFrom(s => GetDefaultMapper<User, UserDTO>().Map<IEnumerable<User>, IEnumerable<UserDTO>>(s.Users)));
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }
        private IMapper GetFromAppDtoToAppMapper(ICollection<UserDTO> usersDTO)
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

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>()
                    .ForMember(s => s.Location, opt => opt.MapFrom(loc => _locations.FindById(loc.LocationId)))
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => users));
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }
        //private IMapper GetFromLocToLocDtoIgnoreMapper()
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<Location, LocationDTO>()
        //            .ForMember(d => d.Appointments,opt => opt.Ignore());
        //    });
        //    IMapper mapper = config.CreateMapper();

        //    return mapper;
        //}
        private IMapper GetFromLocationToLocationDtoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Location, LocationDTO>()
                    .ForMember(d => d.Appointments,
                        opt => opt.MapFrom(s =>
                            GetFromAppToAppDtoMapper()
                                .Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(s.Appointments)));
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        public BLLServiceMain(IGenericRepository<Appointment> appointments, IGenericRepository<User> users, IGenericRepository<Location> locations)
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
            var mappingItem = GetFromLocationToLocationDtoMapper().Map<Location, LocationDTO>(location);
            return mappingItem;
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var locationsMapper =
                GetFromLocationToLocationDtoMapper().Map<IEnumerable<Location>, IEnumerable<LocationDTO>>(_locations.Get());
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

        public void AddAppointment(AppointmentDTO appointment)
        {
            var appointmentItem = GetFromAppDtoToAppMapper(appointment.Users).Map<AppointmentDTO, Appointment>(appointment);

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

        public void AddLocation(LocationDTO location)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LocationDTO, Location>();
            });
            IMapper mapper = config.CreateMapper();

            var locationItem = mapper.Map<LocationDTO, Location>(location);

            using (var transaction = _locations.BeginTransaction())
            {
                try
                {
                    _locations.Create(locationItem);
                    _locations.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void AddLocation(Location location)
        {
            using (var transaction = _locations.BeginTransaction())
            {
                try
                {
                    _locations.Create(location);
                    _locations.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void AddUser(UserDTO user)
        {
            var userItem = GetDefaultMapper<UserDTO, User>().Map<UserDTO, User>(user);

            using (var transaction = _users.BeginTransaction())
            {
                try
                {
                    _users.Create(userItem);
                    _users.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void AddUser(User user)
        {
            using (var transaction = _users.BeginTransaction())
            {
                try
                {
                    _users.Create(user);
                    _users.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void RemoveAppointment(AppointmentDTO appointment)
        {
            var appointmentItem = GetFromAppDtoToAppMapper(appointment.Users).Map<AppointmentDTO, Appointment>(appointment);

            using (var transaction = _appointments.BeginTransaction())
            {
                try
                {
                    _appointments.Remove(appointmentItem, key => key.AppointmentId);
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
