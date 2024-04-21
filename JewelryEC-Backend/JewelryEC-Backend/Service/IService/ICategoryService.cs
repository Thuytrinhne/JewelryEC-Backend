using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Categories.Dto;
using JewelryEC_Backend.Service.Generics;

namespace JewelryEC_Backend.Service.IService
{
    public interface ICategoryService: IGenericCrudOperationService<Category>
    {
        Task<ResponseDto> Add(CreateNewCategoryDto category);
        Task<ResponseDto> MultiAdd(CreateNewCategoryDto[] categories);
        Task<ResponseDto> Delete(Guid categoryId);
        Task<ResponseDto> Update(UpdateCategoryDto category, Guid id);
    }
}
