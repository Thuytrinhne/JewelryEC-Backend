using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Filters;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Products.Dto;
using JewelryEC_Backend.Models.Products;
using Microsoft.AspNetCore.Mvc;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductApiController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }


        [HttpGet]
        // theem query param
        public async Task<ActionResult<ResponseDto>> Get()
        {
            try
            {
                IEnumerable<Product> objList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductItemResponseDto>>(objList);
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{id:Guid}", Name = "GetProductById")]

        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
            try
            {
                Product? obj = _db.Products.FirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductItemResponseDto>(obj);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // fn f2
        //global exception filter in .net core web api (try catch )
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<ResponseDto>>Post([FromBody] CreateProductItemDto CreateProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(CreateProductDto);
                obj.Id = Guid.NewGuid();
                _db.Products.Add(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductItemResponseDto>(obj);
                return CreatedAtRoute("GetProductById", new { id = obj.Id }, _response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPut]
        public async Task<ActionResult<ResponseDto>> Put([FromBody] UpdateProductItemDto updateProductDto)
        {
            try
            {
                Product obj = _mapper.Map<Product>(updateProductDto);
                _db.Products.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductItemResponseDto>(obj);
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() }; ;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.Id == id);
                _db.Products.Remove(obj);
                _db.SaveChanges();
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
