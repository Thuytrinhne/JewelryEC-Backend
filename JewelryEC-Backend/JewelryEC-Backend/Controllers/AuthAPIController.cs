using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Service;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace JewelryEC_Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IAuthService _authService;
        private ResponseDto _response;

        public AuthAPIController(IAuthService authService, IEmailSender emailSender)
        {
            _authService = authService;
            _response = new ResponseDto();
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody]RegistrationDto registrationDto)
        {
            var errorMessage = await  _authService.Register(registrationDto);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { errorMessage };
                return BadRequest(_response);
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
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Id, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { "Error encountered" };
                return BadRequest(_response);
            }
            return Ok(_response);

        }
        [HttpPost("SendingOTP")]
        public async Task<IActionResult> sendingOTP(string email)
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            string subject = "Sending OTP";
            string message = "Your OTP is "+ otp;
            await _emailSender.SendEmailAsync(email, subject, message);
            return Ok(_response);

        }
        // forgot password
        // làm riêng 1 cái cập nhật khách hàng 

    }
}
