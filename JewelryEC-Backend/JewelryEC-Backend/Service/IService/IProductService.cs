using JewelryEC_Backend.Core.Filter;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JewelryEC_Backend.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAll(int pageNumber, int pageSize);
        Task<ResponseDto> GetById(Guid id);
        Task<ResponseDto> Add(CreateProductDto product);
        Task<ResponseDto> Update(UpdateProductDto product);
        Task<ResponseDto> Delete(Guid product);
        Task<ResponseDto> Get(RootFilter? filters, int pageNumber, int pageSize);
    }
}
