using AutoMapper;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Service.IService;
using System.Text.Json.Serialization;
using System.Text.Json;
using JewelryEC_Backend.Core.Filter;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetAll(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            string filter = HttpContext.Request.Query["filter"];
            var filterResult = new RootFilter();

            if (!string.IsNullOrEmpty(filter))
            {
                filterResult = JsonConvert.DeserializeObject<RootFilter>(filter);
            }
            var result= await  _productService.Get(filterResult, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productService.GetById(id);
            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Add([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.Add(productDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("update")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Update(
            [FromBody] UpdateProductDto productDto)
        {
            var result = await _productService.Update( productDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("delete/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            var result = await _productService.Delete(productId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }   

}
