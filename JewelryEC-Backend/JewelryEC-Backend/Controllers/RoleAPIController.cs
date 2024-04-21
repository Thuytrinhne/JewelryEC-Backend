using AutoMapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Roles.Dto;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JewelryEC_Backend.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleAPIController : Controller
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private IRoleService _roleService;

        public RoleAPIController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
            _response = new ResponseDto();
        }


        [HttpGet("")]
        public async Task<ActionResult<ResponseDto>> Index()
        {
            try
            {
                var roles = _roleService.ListRoles();
                if (roles != null && roles.Count() > 0)
                {
                    _response.Result = _mapper.Map<IEnumerable<GetRoleResponseDto>>(roles);
                    return Ok(_response);
                }
                return NotFound("No roles in system");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }


        [HttpGet("{id:Guid}", Name = "GetRoleDetails")]

        public async Task<ActionResult<ResponseDto>> Details([FromRoute] Guid id)
        {
            try
            {
                var roles = _roleService.GetRoleById(id);
                if (roles != null)
                {
                    _response.Result = _mapper.Map<GetRoleResponseDto>(roles);
                    return Ok(_response);
                }
                return NotFound("No role found");
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }

        }

        [HttpPost("")]
        public async Task<ActionResult<ResponseDto>> Create(CreateRoleDto createRoleDto)
        {
            try
            {
                var roleEntity = _mapper.Map<ApplicationRole>(createRoleDto);
                _roleService.CreateRole(roleEntity);
                _response.Result = _mapper.Map<CreateRoleResponseDto>(roleEntity);
                return CreatedAtRoute("GetRoleDetails", new { id = roleEntity.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }



        [HttpPatch("")]
        public async Task<ActionResult<ResponseDto>> Edit(UpdateRoleDto updateRoleDto)
        {
            try
            {
                var roleEntity = _mapper.Map<ApplicationRole>(updateRoleDto);
                var result = _roleService.UpdateRole(roleEntity);
                if (result)
                {
                    _response.Result = _mapper.Map<UpdateRoleResponseDto>(roleEntity);
                    return Ok(_response);
                }
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }
        [HttpDelete("{id:Guid}")]
        public async  Task <ActionResult<ResponseDto>> Delete(Guid id)
        {
            try
            {
                var roleEntity = _roleService.GetRoleById(id);
                if (roleEntity == null)
                    return BadRequest("No role found");
                _roleService.DeleteRole(roleEntity);     
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }
    }



    


}
