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
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Categories.Dto;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;

namespace JewelryEC_Backend.Mapper
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
            CreateMap<GetCartResponseDto, Cart>().ReverseMap();
            CreateMap<CreateCartItemDto, Cart>().ReverseMap();
            // cart item 
            CreateMap<CartItem, GetCartItemResponseDto>().ReverseMap();
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






            CreateMap<ProductCoupon, CreateProductCouponDto>().ReverseMap();
            CreateMap<CatalogCoupon, CreateNewCategoryDto>().ReverseMap();
            CreateMap<ProductCoupon, UpdateProductCouponDto>().ReverseMap();
            //Product
                //CreateMap<Product, CreateProductDto>().ReverseMap();
                //CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}
