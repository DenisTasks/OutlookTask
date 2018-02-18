using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Model.Entities;
using Model.Interfaces;

namespace BLL
{
    public class BLLService : IBLLService
    {
        private IUnitOfWork Database { get; }
        private IMapper FromUserToUserDtoMapper { get; set; }
        private IMapper FromUserDtoToUserMapper { get; set; }
        private IMapper FromAppToAppDtoMapper { get; set; }
        private IMapper FromAppDtoToAppMapper { get; set; }
        private IMapper FromLocationToLocationDtoMapper { get; set; }
        public BLLService(IUnitOfWork uOw)
        {
            Database = uOw;

            //check
            FromUserToUserDtoMapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()
                .ForMember("UserId", opt => opt.MapFrom(s => s.UserId))
                .ForMember("IsActive", opt => opt.MapFrom(s => s.IsActive))
                .ForMember("Name", opt => opt.MapFrom(s => s.Name))
                .ForMember("UserName", opt => opt.MapFrom(s => s.UserName))
                .ForMember("Password", opt => opt.MapFrom(s => s.Password))
            ).CreateMapper();

            //check
            FromUserDtoToUserMapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>()
                .ForMember("UserId", opt => opt.MapFrom(s => s.UserId))
                .ForMember("IsActive", opt => opt.MapFrom(s => s.IsActive))
                .ForMember("Name", opt => opt.MapFrom(s => s.Name))
                .ForMember("UserName", opt => opt.MapFrom(s => s.UserName))
                .ForMember("Password", opt => opt.MapFrom(s => s.Password))
            ).CreateMapper();

            // check
            FromAppToAppDtoMapper = new MapperConfiguration(cfg => cfg.CreateMap<Appointment, AppointmentDTO>()
                .ForMember("AppointmentId", opt => opt.MapFrom(s => s.AppointmentId))
                .ForMember("Subject", opt => opt.MapFrom(s => s.Subject))
                .ForMember("BeginningDate", opt => opt.MapFrom(s => s.BeginningDate))
                .ForMember("EndingDate", opt => opt.MapFrom(s => s.EndingDate))
                .ForMember("LocationId", opt => opt.MapFrom(s => s.LocationId))
                .ForMember(d => d.Users, opt => opt.MapFrom(s => FromUserToUserDtoMapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(s.Users)))
            ).CreateMapper();

            //check
            FromAppDtoToAppMapper = new MapperConfiguration(cfg => cfg.CreateMap<AppointmentDTO, Appointment>()
                .ForMember("AppointmentId", opt => opt.MapFrom(s => s.AppointmentId))
                .ForMember("Subject", opt => opt.MapFrom(s => s.Subject))
                .ForMember("BeginningDate", opt => opt.MapFrom(s => s.BeginningDate))
                .ForMember("EndingDate", opt => opt.MapFrom(s => s.EndingDate))
                .ForMember("LocationId", opt => opt.MapFrom(s => s.LocationId))
                .ForMember(d => d.Users, opt => opt.MapFrom(s => FromUserDtoToUserMapper.Map<IEnumerable<UserDTO>, IEnumerable<User>>(s.Users)))
            ).CreateMapper();

            //check
            FromLocationToLocationDtoMapper = new MapperConfiguration(cfg => cfg.CreateMap<Location, LocationDTO>()
                .ForMember("LocationId", opt => opt.MapFrom(s => s.LocationId))
                .ForMember("Room", opt => opt.MapFrom(s => s.Room))
                .ForMember(d => d.Appointments, opt => opt.MapFrom(s => FromAppToAppDtoMapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(s.Appointments)))
            ).CreateMapper();
        }

        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            List<Appointment> collection;
            using (Database.BeginTransaction())
            {
                collection = Database.Appointments.Get().ToList();
            }
            var mappingCollection = FromAppToAppDtoMapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(collection);
            return mappingCollection;
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            Appointment appointment;
            using (Database.BeginTransaction())
            {
                appointment = Database.Appointments.FindById(id);
            }
            var mappingItem = FromAppToAppDtoMapper.Map<Appointment, AppointmentDTO>(appointment);
            return mappingItem;
        }

        public LocationDTO GetLocationById(int id)
        {
            Location location;
            using (Database.BeginTransaction())
            {
                location = Database.Locations.FindById(id);
            }
            var mappingItem = FromLocationToLocationDtoMapper.Map<Location, LocationDTO>(location);
            return mappingItem;
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var locationsMapper =
                FromLocationToLocationDtoMapper.Map<IEnumerable<Location>, IEnumerable<LocationDTO>>(Database.Locations
                    .Get());
            return locationsMapper;
        }

        public IEnumerable<AppointmentDTO> GetAppsByLocation(AppointmentDTO appointment)
        {
            var appointmentItem = FromAppDtoToAppMapper.Map<AppointmentDTO, Appointment>(appointment);
            return FromAppDtoToAppMapper.Map<IEnumerable<Appointment>, IEnumerable<AppointmentDTO>>(Database.Appointments.Get(x => x.LocationId == appointmentItem.LocationId));
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return FromUserToUserDtoMapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(Database.Users.Get());
        }

        public void AddAppointment(AppointmentDTO appointment)
        {
            var appointmentItem = FromAppDtoToAppMapper.Map<AppointmentDTO, Appointment>(appointment);

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

            var locationsMapper = new MapperConfiguration(cfg => cfg.CreateMap<LocationDTO, Location>()
                .ForMember("LocationId", opt => opt.MapFrom(s => s.LocationId))
                .ForMember("Room", opt => opt.MapFrom(s => s.Room))
                .ForMember(d => d.Appointments, opt => opt.MapFrom(s => FromAppDtoToAppMapper.Map<IEnumerable<AppointmentDTO>, IEnumerable<Appointment>>(s.Appointments)))
            ).CreateMapper();
            var locationItem = locationsMapper.Map<LocationDTO, Location>(location);

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
            var userItem = FromUserDtoToUserMapper.Map<UserDTO, User>(user);

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
            var appointmentItem = FromAppDtoToAppMapper.Map<AppointmentDTO, Appointment>(appointment);

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
                    throw new Exception(e.ToString() + " from BLL");
                }
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
