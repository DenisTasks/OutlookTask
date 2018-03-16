using AutoMapper;
using BLL.EntitesDTO;
using Model.Entities;
using ViewModel.Models;

namespace ViewModel.Helpers
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Appointment, AppointmentDTO>();
                cfg.CreateMap<AppointmentDTO, AppointmentModel>();
                cfg.CreateMap<AppointmentModel, AppointmentDTO>();
                cfg.CreateMap<AppointmentDTO, Appointment>();

                cfg.CreateMap<Location, LocationDTO>();

                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<UserModel, UserDTO>();

                cfg.CreateMap<Group, GroupDTO>();
                cfg.CreateMap<GroupModel, GroupDTO>();
                cfg.CreateMap<GroupDTO, Group>();

                cfg.CreateMap<Role, RoleDTO>();
                cfg.CreateMap<RoleDTO, Role>();

                cfg.CreateMap<Log, LogDTO>();
            });
        }
    }
}