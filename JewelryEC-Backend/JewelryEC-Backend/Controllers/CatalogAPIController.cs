using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Filters;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/catalogs")]
    [ApiController]
    [Authorize]
    public class CatalogAPIController : ControllerBase
    {
      
        private ResponseDto _response;
        private IMapper _mapper;
        private ICatalogService _catalogService;
        public CatalogAPIController( IMapper mapper,ICatalogService catalogService )
        {
            _mapper = mapper;
            _response = new ResponseDto();
            _catalogService = catalogService;
        }


        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] Guid ? parentId, [FromQuery] string ? name)
        {
            try
            {
                IEnumerable<Catalog> objList;
                objList = _catalogService.FilterCatalogs(parentId, name);
                if(objList == null  ||  objList.Count() == 0)
                    return NotFound("Parent Id is not valid or no catalog with this parentId");
                

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
 
                Catalog ?obj = _catalogService.GetCatalogById(id);
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

                if (_catalogService.CreateCatalog(obj)== false)
                {
                    return BadRequest("error occur");
                }
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
                _catalogService.UpdateCatalog(obj);
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
                if (!_catalogService.DeleteCatalog(id))
                    return BadRequest("Catalog belongs to other catalog or error occurred");
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
