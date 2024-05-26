using AutoMapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Users.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] Guid ? roleId)
        {
            try
            {
                IEnumerable<ApplicationUser> users;
                if (roleId.HasValue)
                {
                     users = await _userService.ListUsers(roleId.Value);
                }
                else
                {
                     users = await _userService.ListUsers(Guid.Empty);
                }
                    if (users == null || users.Count() == 0)
                    {
                        return NotFound("No users found [with the specified role].");
                    }
                    var GetUsersDto = new List<GetUserResponseDto>();
                    foreach (var user in users)
                    {

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
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }
       
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<ResponseDto>> GetById([FromRoute] Guid userId)
        {
            try
            {
                var user = _userService.GetUserById(userId);
                if(user != null)
                {
                   
                    var userRoles = await _userService.GetRolesAsync(user);
                    var userDto = _mapper.Map<GetUserResponseDto>(user);
                    userDto.Roles = userRoles.ToList();
                    _response.Result = userDto;
                    return Ok(_response);
                }
                _response.IsSuccess = false;
                _response.Message = "No user found";

                return NotFound(_response);
            }
            catch (Exception ex)
            {


                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);


            }
        }
        [Authorize]
        [HttpPatch("{userId}")]
        [Authorize]

        public async Task<ActionResult<ResponseDto>> Patch(Guid userId, [FromBody] UpdateUserDto updateUser)
        {
            try
            {
                if (checkValidUserId(userId))
                {
                    var user = _userService.GetUserById(userId);
                    if (user != null)
                    {                  
                        _userService.EditProfile(userId, updateUser);
                        _response.Result = _mapper.Map<UpdateUserResponseDto>(user);
                        return Ok(_response);
                    }
                }
                        return NotFound("No user found"); 

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }
        }
        private bool checkValidUserId(Guid userId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim != userId.ToString())
            {
                return false;
            }
            return true;
        }

        [HttpPost("{userId}/assignRole")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto, Guid userId)
        {
            var assignRoleSuccessful = await _userService.AssignRole(userId, assignRoleDto.RoleId);
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "RoleId or UserId is not valid";
                return BadRequest(_response);
            }
            return Ok(_response);

        }


    }
}
