using AutoMapper;
using JewelryEC_Backend.Core.Pagination;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Users.Dto;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
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
        private IPhotoCloudService _photoCloudService;
        private IPhotoService _photoService;

        public UserAPIController(IMapper mapper, IUserService userService,
            IPhotoCloudService photoCloudService, IPhotoService photoService)
        {
            _mapper = mapper;
            _response = new ResponseDto();
            _userService = userService;
            _photoCloudService = photoCloudService;
            _photoService = photoService;
        }
        //[HttpGet("search")]
        //public async Task<ActionResult<ResponseDto>> Search ([FromQuery] string? keyword)
        //{
        //    if(!string.IsNullOrEmpty(keyword))
        //    { 
        //         var result =  await  _userService.SearchRecordsAsync(keyword);
        //        if (result.Count != 0)
        //        {
        //            _response.Result = _mapper;
        //            return Ok(_response);

        //        }
        //    }

        //    _response.IsSuccess = false;
        //    _response.Message = $"Don't have any users match with {keyword}";

        //    return NotFound();

        //}
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get([FromQuery] PaginationRequest paginationRequest,
            [FromQuery] Guid? roleId, [FromQuery] string? keyword, string name = null, string phone = null)
        {
            try
            {
                PaginationResult<ApplicationUser> result;
                if (roleId.HasValue)
                {
                    result = await _userService.SearchRecordsAsyncPagination
                    (paginationRequest, roleId.Value, keyword);
                }
                else result = await _userService.SearchRecordsAsyncPagination
                (paginationRequest, keyword: keyword);

                if (result.Data == null || result.Data.Count() == 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Not found user";

                    return NotFound(_response);
                }



                var GetUsersDto = new List<GetUserResponseDto>();
                foreach (var user in result.Data)
                {

                    // Lấy role của người dùng
                    var userRoles = await _userService.GetRolesAsync(user);

                    var GetUserDto = _mapper.Map<GetUserResponseDto>(user);
                    GetUserDto.Roles = userRoles.ToList();
                    GetUsersDto.Add(GetUserDto);
                }

                PaginationResult<GetUserResponseDto> resultResponse = new PaginationResult<GetUserResponseDto>(
                          result.PageIndex,
                          result.PageSize,
                          result.Count,
                          GetUsersDto
                        );


                _response.Result = resultResponse;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
                return StatusCode(500, _response);
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> GetById([FromRoute] Guid id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user != null)
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
        [HttpPatch("{id}")]

        public async Task<ActionResult<ResponseDto>> Patch(Guid id, [FromBody] UpdateUserDto updateUser)
        {
            try
            {
                if (checkValidUserId(id))
                {
                    var user = _userService.GetUserById(id);
                    if (user != null)
                    {
                        _userService.EditProfile(id, updateUser);
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

        [HttpPost("{id}/role")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto, Guid id)
        {
            if (!checkValidUserId(id))
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is invalid.";
                return BadRequest(_response);
            }
            var assignRoleSuccessful = await _userService.AssignRole(id, assignRoleDto.RoleId);
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "RoleId or UserId is not valid";
                return BadRequest(_response);
            }
            return Ok(_response);

        }

        [HttpPost("upload")]
        public async Task<IActionResult> GetImgUrl(IFormFile file)
        {
            var result = await _photoCloudService.AddPhotoAsync(file);
            if (result.Error is not null)
            {
                _response.IsSuccess = false;
                _response.Message = result.Error.Message;
                return BadRequest(_response);
            }
            var imgUrl = result.SecureUri.AbsoluteUri;
            _response.Result = imgUrl;
            return Ok(_response);
        }

        [HttpPost("{id}/avatar")]
        [Authorize]

        public async Task<IActionResult> UpsertPhoto(IFormFile file, Guid id)
        {
            // check userId trùng token?
            if (!checkValidUserId(id))
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is invalid.";
                return BadRequest(_response);
            }
            var user = _userService.GetUserById(id);

            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                await _photoCloudService.DeletePhotoAsync(user.PublicId);
            }
            var result = await _photoCloudService.AddPhotoAsync(file);
            if (result.Error is not null)
            {
                _response.IsSuccess = false;
                _response.Message = result.Error.Message;
                return BadRequest(_response);
            }
            var imgUrl = result.SecureUri.AbsoluteUri;
            _photoService.AddPhotoAsync(id, imgUrl, result.PublicId);

            _response.Result = imgUrl;
            return Ok(_response);

        }
        [HttpDelete("{id}/avatar")]
        [Authorize]
        public async Task<IActionResult> DeletePhoto(Guid id)
        {
            // check userId trùng token?
            if (!checkValidUserId(id))
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is invalid.";
                return BadRequest(_response);
            }
            var user = _userService.GetUserById(id);

            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var result = await _photoCloudService.DeletePhotoAsync(user.PublicId);
                if (result.Error is not null)
                {
                    _response.IsSuccess = false;
                    _response.Message = result.Error.Message;
                    return BadRequest(_response);
                }
            }

            _photoService.DeletePhotoAsync(id);
            return Ok(_response);

        }

        [HttpPatch("{id}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(Guid id, ChangePasswordDto changePasswordDto)
        {
            // check userId trùng token?
            if (!checkValidUserId(id))
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is invalid.";
                return BadRequest(_response);
            }
            var user = _userService.GetUserById(id);
            if (await _userService.UpdatePassword(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword))
            {
                return Ok(_response);

            }
            _response.IsSuccess = false;
            _response.Message = "Can not change your password. Please try again. ";

            return BadRequest(_response);

        }
    }
}
