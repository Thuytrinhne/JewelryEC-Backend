using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Service.Generics;

namespace JewelryEC_Backend.Service.IService
{
    public interface ICategoryService: IGenericCrudOperationService<Category>
    {
        Task<ResponseDto> Add(Category category);
        Task<ResponseDto> MultiAdd(Category[] categories);
    }
}
