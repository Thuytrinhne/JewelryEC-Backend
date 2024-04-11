using System.Threading.Tasks;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(Category category)
        {
            var result = await _categoryService.Add(category);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("multiadd")]
        public async Task<IActionResult> MultiAdd(Category[] categories)
        {
            var result = await _categoryService.MultiAdd(categories);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(Category category)
        {
            var result = await _categoryService.Delete(category);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(Category category)
        {
            var result = await _categoryService.Update(category);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}