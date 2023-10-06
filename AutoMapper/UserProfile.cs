using AutoMapper;
using MiApi.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.TipoPersona, opt => opt.MapFrom(src => new TipoPersonaDTO
            {Id = src.TipoPersonaId,Nombre = src.TipoPersona.Nombre}));
        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>();
        CreateMap<TipoPersona, TipoPersonaDTO>().ReverseMap();
        CreateMap<UpdateTipoPersonaDTO, TipoPersona>();
        CreateMap<Role, RoleDTO>().ReverseMap();
        CreateMap<CreateProductDTO, Product>();
        CreateMap<CreateMidierDTO, Midier>();
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<DeleteOrderDTO, Order>();
        CreateMap<CreateOrderProductDTO, OrderProduct>();
        CreateMap<DeleteOrderProductDTO, OrderProduct>();
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        CreateMap<Midier, MidierDTO>();
        CreateMap<UpdateProductDTO, Product>();
        CreateMap<Order, OrderReadDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))  // Mapea el nombre del usuario
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.OrderProducts.Select(op => op.Product)));
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<UpdateOrderDTO, Order>();

    }
}
