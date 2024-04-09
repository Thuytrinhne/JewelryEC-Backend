using AutoMapper;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // catalog
            CreateMap<Catalog, CreateCatalogDto>().ReverseMap();
            CreateMap<Catalog, CreateCatalogResponseDto>().ReverseMap();
            CreateMap<Catalog, GetCatalogResponseDto>().ReverseMap();
            CreateMap<Catalog, UpdateCatalogDto>().ReverseMap();
            CreateMap<Catalog, UpdateCatalogResponseDto>().ReverseMap();
            // auth
            CreateMap<ApplicationUser, RegistrationResponseDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            // cart
            CreateMap<GetCartDto, Cart>().ReverseMap();
            CreateMap<CreateCartDto, Cart>().ReverseMap();
            // cart item 
            CreateMap<CartItem, GetCartItemDto>().ReverseMap();


        }
    }
}
