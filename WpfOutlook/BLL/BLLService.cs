using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Model;
using Model.Entities;
using Model.Interfaces;
using Model.ModelVIewElements;

namespace BLL
{
    public class BLLService : IBLLService
    {
        private IUnitOfWork Database { get; }
        //
        private ITestInterface<Appointment> _appointments;
        private ITestInterface<User> _users;
        private ITestInterface<Location> _location;

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
                if (Database.Users.FindById(item.UserId) != null)
                {
                    users.Add(Database.Users.FindById(item.UserId));
                }
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>()
                    .ForMember(s => s.Location, opt => opt.MapFrom(loc => Database.Locations.FindById(loc.LocationId)))
                    .ForMember(d => d.Users, opt => opt.MapFrom(s => users));
            });
            IMapper mapper = config.CreateMapper();

            return mapper;
        }
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

        public BLLService(IUnitOfWork uOw, ITestInterface<Appointment> app, ITestInterface<User> users, ITestInterface<Location> loc)
        {
            //_context = new WPFOutlookContext();
            //_users = new GenericRepository<User>(_context);
            //_groups = new GenericRepository<Group>(_context);

            _appointments = app;
            _users = users;
            _location = loc;

            Database = uOw;
        }

        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            List<Appointment> collection;
            using (_appointments.BeginTransaction())
            {
                //collection = Database.Appointments.Get(s => s.Users.Any(d => d.UserId == 92)).ToList();
                collection = _appointments.Get().ToList();
            }

            var mappingCollection = GetFromAppToAppDtoMapper().Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            Appointment appointment;
            using (Database.BeginTransaction())
            {
                appointment = Database.Appointments.FindById(id);
            }
            var mappingItem = GetFromAppToAppDtoMapper().Map<Appointment, AppointmentDTO>(appointment);
            return mappingItem;
        }

        public LocationDTO GetLocationById(int id)
        {
            Location location;
            using (Database.BeginTransaction())
            {
                location = Database.Locations.FindById(id);
            }
            var mappingItem = GetFromLocationToLocationDtoMapper().Map<Location, LocationDTO>(location);
            return mappingItem;
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var locationsMapper =
                GetFromLocationToLocationDtoMapper().Map<IEnumerable<Location>, IEnumerable<LocationDTO>>(Database.Locations
                    .Get());
            return locationsMapper;
        }

        public IEnumerable<AppointmentDTO> GetAppsByLocation(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>()
                    .ForMember(s => s.Location, opt => opt.MapFrom(loc => Database.Locations.FindById(loc.LocationId)));
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(Database.Appointments.Get(x => x.LocationId == id));
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return GetDefaultMapper<User, UserDTO>().Map<IEnumerable<User>, IEnumerable<UserDTO>>(Database.Users.Get());
        }

        public void AddAppointment(AppointmentDTO appointment)
        {
            var appointmentItem = GetFromAppDtoToAppMapper(appointment.Users).Map<AppointmentDTO, Appointment>(appointment);

            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Appointments.Create(appointmentItem);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show(e.ToString());
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

            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Locations.Create(locationItem);
                    Database.Save();
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
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Locations.Create(location);
                    Database.Save();
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

            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Users.Create(userItem);
                    Database.Save();
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
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Users.Create(user);
                    Database.Save();
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

            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.Appointments.Remove(appointmentItem, key => key.AppointmentId);
                    Database.Save();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e + " from BLL");
                }
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
