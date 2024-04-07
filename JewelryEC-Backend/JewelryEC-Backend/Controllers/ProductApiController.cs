//using AutoMapper;
//using JewelryEC_Backend.Data;
//using JewelryEC_Backend.Filters;
//using JewelryEC_Backend.Models;
//using JewelryEC_Backend.Models.Catalogs.Dto;
//using JewelryEC_Backend.Models.Catalogs.Entities;
//using JewelryEC_Backend.Models.Products;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;


//namespace JewelryEC_Backend.Controllers
//{
//    [Route("api/catalog")]
//    [ApiController]
//    public class ProductApiController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        private ResponseDto _response;
//        private IMapper _mapper;

//        public ProductApiController(AppDbContext db, IMapper mapper)
//        {
//            _db = db;
//            _mapper = mapper;
//            _response = new ResponseDto();
//        }


//        [HttpGet]
//        // theem query param
//        public async Task<ActionResult<ResponseDto>> Get()
//        {
//            try
//            {
//                IEnumerable<Catalog> objList = _db.Catalogs.ToList();
//                _response.Result = _mapper.Map<IEnumerable<GetCatalogResponseDto>>(objList);
//                return Ok(_response);

//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() };
//            }
//            return _response;
//        }
//        [HttpGet("{id:Guid}", Name = "GetCatalogById")]

//        public async Task<ActionResult<ResponseDto>> Get(Guid id)
//        {
//            try
//            {

//                Catalog? obj = _db.Catalogs.FirstOrDefault(u => u.Id == id);
//                if (obj == null)
//                {
//                    _response.IsSuccess = false;
//                    return NotFound(_response);
//                }
//                _response.Result = _mapper.Map<GetCatalogResponseDto>(obj);
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() };
//            }
//            return _response;
//        }

//        // fn f2
//        //global exception filter in .net core web api (try catch )
//        [HttpPost]
//        [ValidateModel]
//        public async Task<ActionResult<ResponseDto>> Post([FromBody] CreateCatalogDto CreateCatalogDto)
//        {
//            try
//            {
//                Catalog obj = _mapper.Map<Catalog>(CreateCatalogDto);
//                obj.Id = Guid.NewGuid();
//                _db.Catalogs.Add(obj);
//                _db.SaveChanges();
//                _response.Result = _mapper.Map<CreateCatalogResponseDto>(obj);
//                return CreatedAtRoute("GetCatalogById", new { id = obj.Id }, _response);


//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() };
//            }
//            return _response;
//        }
//        [HttpPut]
//        public async Task<ActionResult<ResponseDto>> Put([FromBody] UpdateCatalogDto updateCatalogDto)
//        {
//            try
//            {
//                Catalog obj = _mapper.Map<Catalog>(updateCatalogDto);
//                _db.Catalogs.Update(obj);
//                _db.SaveChanges();
//                _response.Result = _mapper.Map<UpdateCatalogResponseDto>(obj);
//                return Ok(_response);

//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() }; ;
//            }
//            return _response;
//        }

//        [HttpDelete]
//        [Route("{id:Guid}")]
//        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
//        {
//            try
//            {
//                Product obj = _db.Products.First(u => u.Id == id);
//                _db.Products.Remove(obj);
//                _db.SaveChanges();
//                return Ok(_response);
//            }
//            catch (Exception ex)
//            {
//                _response.IsSuccess = false;
//                _response.ErrorMessages = new List<string>() { ex.ToString() };
//            }
//            return _response;
//        }

//    }
//}
