using AutoMapper;
using MiApi.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.TipoPersona, opt => opt.MapFrom(src => new TipoPersonaDTO
            {
                Id = src.TipoPersonaId,
                Nombre = src.TipoPersona.Nombre
            }));
        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>();
        CreateMap<TipoPersona, TipoPersonaDTO>().ReverseMap();
        CreateMap<UpdateTipoPersonaDTO, TipoPersona>();
    }
}
