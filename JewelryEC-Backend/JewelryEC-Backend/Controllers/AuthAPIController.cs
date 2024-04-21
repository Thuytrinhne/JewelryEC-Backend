using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Utility;
using Asp.Versioning;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;


namespace JewelryEC_Backend.Controllers
{

    [ApiController]
    // [Route("api/v{version:apiVersion}/auth")]
    [Route("api/auth")]
    [ApiVersion("1.0")]


    public class AuthAPIController : ControllerBase
    {       
        private readonly IAuthService _authService;
        private ResponseDto _response;

        public AuthAPIController(IAuthService authService, IEmailSender emailSender)
        {
            _authService = authService;
            _response = new ResponseDto();
     
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody]RegistrationDto registrationDto)
        {
            var errorMessage = await  _authService.Register(registrationDto);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { errorMessage };
                return BadRequest("Error occur");
            }
            
               return Ok(_response);
          
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { "Username or password is incorrect" };
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);

        }
        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto)
        {
            var assignRoleSuccessful = await _authService.AssignRole(assignRoleDto.UserId, assignRoleDto.RoleId);
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { "Error encountered" };
                return BadRequest(_response);
            }
            return Ok(_response);

        }
        [HttpPost("otps")]
        public async Task<IActionResult> sendingOTP(SendOTPDto sendOTPDto)
        {
             if (!await _authService.SendingOTP(sendOTPDto.Email))
                {
                return BadRequest("Error encountered");
                }
            _response.Message = "Check your email to get OTP !!!";
            return Ok(_response);

        }
     
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (await _authService.ForgotPassword(forgotPasswordDto.Email))
            {
                _response.ErrorMessages = new List<string>() { "Check email and change password, please !!!" };
                return Ok(_response);
            }
            else
            {
                _response.ErrorMessages = new List<string>() { "Email is not existed in system !!! Check again !!!" };
                return BadRequest(_response);
            }
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> resetPassword(string token, ResetPasswordDto resetPasswordDto)
        {
            if (await _authService.ResetPassword(token, resetPasswordDto.NewPassword))
           return Ok(_response);
            return BadRequest("Encounter errors");   
        }
        // làm riêng 1 cái cập nhật khách hàng 

    }
}

