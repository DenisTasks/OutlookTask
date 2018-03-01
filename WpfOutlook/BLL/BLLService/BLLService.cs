using AutoMapper;
using BLL.EntitesDTO;
using BLL.Interfaces;
using Model.Entities;
using Model.Interfaces;
using Model.ModelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BLLService
{
    public class BLLService : IBLLService
    {
        private IUnitOfWork _db;

        private IMapper userToUserDTO()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>()
                .ForMember("UserId", opt => opt.MapFrom(s => s.UserId))
                .ForMember("IsActive", opt => opt.MapFrom(s => s.IsActive))
                .ForMember("Name", opt => opt.MapFrom(s => s.Name))
                .ForMember("UserName", opt => opt.MapFrom(s => s.UserName))
                .ForMember("Password", opt => opt.MapFrom(s => s.Password))
                .ForMember("Roles", opt => opt.MapFrom(s => s.Roles));
                //.ForMember("Appointments", opt => opt.MapFrom(s => s.Appointments))
                //.ForMember("Groups", opt => opt.MapFrom(s => s.Groups));
            }).CreateMapper();
            
            return mapper;
        }

        public BLLService()
        {
            _db = new UnitOfWork();
        }

        public void AddAppointment(AppointmentDTO appointment)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>();
            });
            IMapper mapper = config.CreateMapper();
            _db.Appointments.Create(mapper.Map<AppointmentDTO, Appointment>(appointment));
            _db.Save();
        }

        public void AddLocation(LocationDTO location)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LocationDTO, Location>();
            });
            IMapper mapper = config.CreateMapper();
            _db.Locations.Create(mapper.Map<LocationDTO, Location>(location));
            _db.Save();
        }

        public void AddUser(UserDTO user)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
            });
            IMapper mapper = config.CreateMapper();
            _db.Users.Create(mapper.Map<UserDTO, User>(user));
            _db.Save();
        }

        public UserDTO CheckUser(string username, string password)
        {
            return userToUserDTO().Map<User,UserDTO>(_db.Users.Get(u => u.UserName.Equals(username) && u.Password.Equals(password)).FirstOrDefault());
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Appointment, AppointmentDTO>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Appointment>, List<AppointmentDTO>>(_db.Appointments.Get());
        }

        public IEnumerable<AppointmentDTO> GetAppsByLocation(AppointmentDTO appointment)
        {
            throw new NotImplementedException();
        }

        public LocationDTO GetLocationById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LocationDTO> GetLocations()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Location, LocationDTO>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Location>, List<LocationDTO>>(_db.Locations.Get());
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_db.Users.Get());
        }

        public void RemoveAppointment(AppointmentDTO appointment)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppointmentDTO, Appointment>();
            });
            IMapper mapper = config.CreateMapper();
            _db.Appointments.Remove(mapper.Map<AppointmentDTO, Appointment>(appointment));
            _db.Save();
        }
    }
}
