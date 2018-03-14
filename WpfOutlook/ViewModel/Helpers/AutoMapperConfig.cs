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
                cfg.CreateMap<Appointment, AppointmentDTO>()
                    .ForSourceMember(d => d.Location, opt => opt.Ignore());
                cfg.CreateMap<Location, LocationDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<AppointmentModel, AppointmentDTO>().ForMember(s => s.LocationId,
                    opt => opt.MapFrom(loc => loc.LocationId));
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<GroupDTO, Group>();
                cfg.CreateMap<Group, GroupDTO>();
                cfg.CreateMap<RoleDTO, Role>();
                cfg.CreateMap<Role, RoleDTO>();
                cfg.CreateMap<Log, LogDTO>();
                cfg.CreateMap<GroupModel, GroupDTO>();
                cfg.CreateMap<UserModel, UserDTO>();

            });
        }

        //public static TEntityTo DefaultMapperItem<TEntityFrom, TEntityTo>(TEntityFrom model, TEntityTo newModel)
        //{
        //    var mapconfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TEntityFrom, TEntityTo>());
        //    var mapper = mapconfiguration.CreateMapper();
        //    var result = mapper.Map(model, newModel);
        //    return result;
        //}

        //public static IEnumerable<TEntityTo> DefaultMapperCollection<TEntityFrom, TEntityTo>(IEnumerable<TEntityFrom> model, IEnumerable<TEntityTo> newModel)
        //{
        //    var mapconfiguration = new MapperConfiguration(cfg => cfg.CreateMap<TEntityFrom, TEntityTo>());
        //    var mapper = mapconfiguration.CreateMapper();
        //    var result = mapper.Map(model, newModel);
        //    return result;
        //}
    }
}
