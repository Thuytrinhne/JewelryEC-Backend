using AutoMapper;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.CartItems.Dto;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Dto;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Roles.Dto;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Models.Users.Dto;

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
            CreateMap<CreateCartItemDto, Cart>().ReverseMap();
            // cart item 
            CreateMap<CartItem, GetCartItemDto>().ReverseMap();
            CreateMap<CartItem, CreateCartItemDto>().ReverseMap();
            CreateMap<CartItem, CreateCartItemResponseDto>().ReverseMap();
            //user
            CreateMap<ApplicationUser, GetUserResponseDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, UpdateUserResponseDto>().ReverseMap();
            // role
            CreateMap<ApplicationRole, GetRoleResponseDto>().ReverseMap();
            CreateMap<ApplicationRole, CreateRoleResponseDto>().ReverseMap();
            CreateMap<ApplicationRole, CreateRoleDto>().ReverseMap();
            CreateMap<ApplicationRole, UpdateRoleDto>().ReverseMap();
            CreateMap<ApplicationRole, UpdateRoleResponseDto>().ReverseMap();






        }
    }
}
