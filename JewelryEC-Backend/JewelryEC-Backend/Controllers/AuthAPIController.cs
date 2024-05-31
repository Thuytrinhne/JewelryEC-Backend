using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Utility;
using Asp.Versioning;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Authentication;


namespace JewelryEC_Backend.Controllers
{

    [ApiController]
    [Route("api/auth")]


    public class AuthAPIController  : ControllerBase
    {       
        private readonly IAuthService _authService;

        private ResponseDto _response;


        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody]RegistrationDto registrationDto)
        {
            try
            {
                var result = await _authService.Register(registrationDto);
                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.Message = "OTP is not valid";
                    return BadRequest(_response);
                }

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return StatusCode(500, _response); // Trả về 500 Internal Server Error
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var loginResponse = await _authService.Login(model);
                if (loginResponse.User == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Username or password is incorrect";
                    return BadRequest(_response);
                }
         
                _response.Result = loginResponse;
                return Ok(_response);
            }
            catch (AuthenticationException ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
           

        }
       
        [HttpPost("otps")]
        public async Task<IActionResult> sendingOTP(SendOTPDto sendOTPDto)
        {

            bool result = await _authService.SendingOTP(sendOTPDto.Email);
                if(!result)
                {
                    _response.IsSuccess = false;
                    _response.Message = $"Email {sendOTPDto.Email} is already taken";
                    return BadRequest(_response);
                }
            _response.Message =  "Check your email to get OTP !!!";
            return Ok(_response);

        }
     
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (await _authService.ForgotPassword(forgotPasswordDto.Email))
            {
                _response.Message = "Check email and change password, please !!!";
                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Email is not existed in system !!! Check again !!!";
                return BadRequest(_response);
            }
        }
        [HttpPost("resetPassword")]
       
        public async Task<IActionResult> resetPassword([FromHeader] string token, ResetPasswordDto resetPasswordDto)
        {
            
            if (await _authService.ResetPassword(token, resetPasswordDto.NewPassword))
           return Ok(_response);
            _response.IsSuccess = false;
            _response.Message = "Token is invalid or password is not enough strong.";
            return BadRequest(_response);   
        }

    }
}

