using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Service.IService;
using Microsoft.Build.Logging;

namespace JewelryEC_Backend.Service
{
    public class CategoryService: ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryService(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Category>>(await _categoryDal.GetAllAsync());
        }
        public async Task<ResponseDto> Add(Category category)
        {
            await _categoryDal.AddAsync(category);
            return new SuccessResult();
        }
        public async Task<ResponseDto> MultiAdd(Category[] categories)
        {
            await _categoryDal.MultiAddAsync(categories);
            return new SuccessResult();
        }

        public async Task<ResponseDto> Delete(Category category)
        {

            await _categoryDal.Delete(category);
            return new SuccessResult();
        }
        public async Task<ResponseDto> Update(Category category)
        {
            await _categoryDal.Update(category);
            return new SuccessResult();
        }
    }
}
