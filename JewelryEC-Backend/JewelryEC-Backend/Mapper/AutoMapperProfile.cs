using AutoMapper;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Categories.Dto;
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
            //Product
                //CreateMap<Product, CreateProductDto>().ReverseMap();
                //CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}
