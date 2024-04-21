using AutoMapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Users.Dto;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JewelryEC_Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserAPIController : Controller
    {

        private ResponseDto _response;
        private IMapper _mapper;
        private IUserService _userService;
        public UserAPIController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _response = new ResponseDto();
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] string ? roleId)
        {
            try
            {                       
                var  users = await _userService.ListUsers(roleId);
                if (users == null || users.Count() == 0)
                {
                    return NotFound("No users found [with the specified role].");
                }
                var GetUsersDto = new List<GetUserResponseDto>();
                foreach(var user in users) {
                   
                    // Lấy role của người dùng
                    var userRoles = await _userService.GetRolesAsync(user);

                    var GetUserDto = _mapper.Map<GetUserResponseDto>(user);
                    GetUserDto.Roles = userRoles.ToList();
                    GetUsersDto.Add(GetUserDto);
                }
                _response.Result = GetUsersDto;
                return Ok(_response);
            } catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
       
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById([FromRoute] Guid id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if(user != null)
                {
                   
                    var userRoles = await _userService.GetRolesAsync(user);
                    var userDto = _mapper.Map<GetUserResponseDto>(user);
                    userDto.Roles = userRoles.ToList();
                    _response.Result = userDto;
                    return Ok(_response);
                }
                return NotFound("No user found");
            }
            catch (Exception ex)
            {


                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);


            }
        }
        [HttpPut]
        public async Task<ActionResult<ResponseDto>> Put([FromBody] UpdateUserDto updateUser)
        {
            try
            {

                var user = _userService.GetUserById(updateUser.Id);
                if (user != null)
                {

                    _userService.EditProfile(_mapper.Map<ApplicationUser>(updateUser));
                    _response.Result = _mapper.Map<UpdateUserResponseDto>(user);
                    return Ok(_response);
                }
                else
                    return NotFound("No user found");

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
