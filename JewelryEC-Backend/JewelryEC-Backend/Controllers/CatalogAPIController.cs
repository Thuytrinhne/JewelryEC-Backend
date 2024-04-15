using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Filters;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    [Authorize]
    public class CatalogAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public CatalogAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }


        [HttpGet]
       
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] Guid ? parentId, [FromQuery] string ? name)
        {
            try
            {
                IEnumerable<Catalog> objList;
                if (parentId.HasValue && !string.IsNullOrEmpty(name))
                {
                    objList = await _db.Catalogs.Where(c => c.ParentId == parentId && c.Name == name.Trim()).ToListAsync();
                }
                else if (parentId.HasValue)
                {
                     objList = await _db.Catalogs.Where(c => c.ParentId == parentId).ToListAsync();
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    objList = await _db.Catalogs.Where(c => c.Name == name.Trim()).ToListAsync();
                }
                else
                {
                    objList = _db.Catalogs.ToList();
                }
                _response.Result = _mapper.Map<IEnumerable<GetCatalogResponseDto>>(objList);
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);

            }
        }
        [HttpGet("{id:Guid}", Name = "GetCatalogById")]
        public async Task<ActionResult<ResponseDto>> Get(Guid id)
        {
            try
            {
 
                Catalog ?obj = _db.Catalogs.FirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }    
                _response.Result = _mapper.Map<GetCatalogResponseDto>(obj);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }
            return _response;
        }
       
     
        //global exception filter in .net core web api (try catch )
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Post([FromBody] CreateCatalogDto CreateCatalogDto)
        {
            try
            {
                Catalog obj = _mapper.Map<Catalog>(CreateCatalogDto);
                obj.Id = Guid.NewGuid(); 
                _db.Catalogs.Add(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CreateCatalogResponseDto>(obj);
                return CreatedAtRoute("GetCatalogById", new { id = obj.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }
            return _response;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Put([FromBody] UpdateCatalogDto updateCatalogDto)
        {
            try
            {
                Catalog obj = _mapper.Map<Catalog>(updateCatalogDto);
                _db.Catalogs.Update(obj);
                _db.SaveChanges();
                _response.Result = _mapper.Map<UpdateCatalogResponseDto>(obj);
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Delete(Guid id)
        {
            try
            {
                Catalog obj =  _db.Catalogs.First(u => u.Id == id);
                _db.Catalogs.Remove(obj);
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
