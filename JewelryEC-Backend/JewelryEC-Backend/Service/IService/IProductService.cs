using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Products.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> GetById(Guid id);
        Task<ResponseDto> Add(CreateProductDto product);
        Task<ResponseDto> Update(UpdateProductDto product);
        Task<ResponseDto> Delete(Guid product);
    }
}
