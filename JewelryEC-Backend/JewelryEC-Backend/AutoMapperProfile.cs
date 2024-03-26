using AutoMapper;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;

namespace JewelryEC_Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Catalog, CreateCatalogDto>().ReverseMap();
            CreateMap<Catalog, CreateCatalogResponseDto>().ReverseMap();
            CreateMap<Catalog, GetCatalogResponseDto>().ReverseMap();
            CreateMap<Catalog, UpdateCatalogDto>().ReverseMap();
            CreateMap<Catalog, UpdateCatalogResponseDto>().ReverseMap();

        }
    }
}
