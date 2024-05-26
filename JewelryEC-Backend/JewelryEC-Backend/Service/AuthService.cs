using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using JewelryEC_Backend.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Security.Authentication;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace JewelryEC_Backend.Service
{
    public class AuthService : IAuthService
    {
    
        private IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IService.IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService  _userService;

        public AuthService(
            IMapper mapper, IJwtTokenGenerator jwtTokenGenerator,
            IService.IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IUserService userService)
        {          
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }



        public async Task<bool> Register(RegistrationDto registrationDto)
        {
            if (checkOTP(registrationDto.Email, registrationDto.OTP))
            {
                var applicationUser = CreateApplicationUser(registrationDto);
                try
                {

                    var result = await AddUser(applicationUser, registrationDto.Password);
                    if (result.Succeeded)
                    {
                        Guid userRoleId = new Guid("10ebc6bb-244f-4180-8804-bb1afd208866");
                        await _userService.AssignRole(applicationUser.Id, userRoleId);
                        return true;
                    }
                    else
                    {
                        throw new Exception(result.Errors.FirstOrDefault().Description);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not create user {applicationUser.UserName}: {ex.Message}");
                }
            }
            else
            {
                return false;
            }
        }

      
        // có thêm vào interface k 
        private ApplicationUser CreateApplicationUser(RegistrationDto registrationDto)
        {
            return new ApplicationUser
            {
                UserName = registrationDto.Email.ToLower(),
                Email = registrationDto.Email,
                NormalizedEmail = registrationDto.Email.ToUpper(),
                Name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber
            };
        }
        // có thêm vào interface k 

        private async Task<IdentityResult> AddUser(ApplicationUser applicationUser, string password)
        {
            return await _unitOfWork.Users.AddUserByUserManager(applicationUser, password);
        }
   
        public bool checkOTP(string email, string otp)
        {
            var objFrmDb = _unitOfWork.EmailVerifications.GetEntityByEmail(email);
            if (objFrmDb != null && objFrmDb.Otp == otp)
            {
               return  checkExpiryOTP(objFrmDb.Created_at) == true ? true : false;
            }
            return false;
        }

        private bool checkExpiryOTP(DateTime created_at)
        {
            var currentTime = DateTime.UtcNow;
            if (created_at.AddMinutes(SD.OTPValidTime_Mins) >= currentTime)
            {
                return true;
            }
            return false;
        }
        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = await  _unitOfWork.Users.GetUserByEmail(loginDto.Email);
            if(user is not null)
            {
                bool isValid = await _unitOfWork.Users.Login(user, loginDto.Password);
                if ( isValid)
                {
                    //if user was found , Generate JWT Token
                    var roles = await _unitOfWork.Users.GetRoleAsync(user);
                    if(roles is null || roles.Count() ==0)
                    {
                        throw new AuthenticationException("Người dùng không có quyền truy cập.");
                    }
                    var token = _jwtTokenGenerator.GenerateToken(user, roles);

                    var userDTO = _mapper.Map<UserDto>(user);

                    LoginResponseDto loginResponseDto = new LoginResponseDto()
                    {
                        User = userDTO,
                        Token = token
                    };
                    return loginResponseDto;
                }

            }
            throw new AuthenticationException("Email hoặc mật khẩu không chính xác.");

        }
       

        public async Task<bool> SendingOTP(string email)
        {
            try
            {
                var user = await  _unitOfWork.Users.GetUserByEmail(email);
                if (user is not null)
                {
                    return false;
                }
                string otp = GenerateOTP();
                await SendOTPEmail(email, otp);
                await SaveEmailVerification(email, otp);
                return true;
            }
            catch (Exception ex)
            {
                // Log exception here if you have a logging mechanism
                throw new Exception("An error occurred while sending OTP: " + ex.Message, ex);
            }
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private async Task SendOTPEmail(string email, string otp)
        {
            string subject = "JewelryStore- Sending OTP";
            string message = $"Your OTP is {otp}.\n Note that this OTP is valid within {SD.OTPValidTime_Mins.ToString()} mins.";
            await _emailSender.SendEmailAsync(email, subject, message);
        }
        private async Task SaveEmailVerification(string email, string otp)
        {
            EmailVerification emailVerification = new EmailVerification
            {
                Id = Guid.NewGuid(),
                Email = email,
                Otp = otp
            };

            _unitOfWork.EmailVerifications.Add(emailVerification);
            _unitOfWork.Save();
        }


       
        public async Task<bool> ForgotPassword(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetUserByEmail(email);
                if (user is null)
                {
                    return false;
                }
                var roles = await _unitOfWork.Users.GetRoleAsync(user);
                if (roles is null || !roles.Any())
                {
                    throw new AuthenticationException("Người dùng không có quyền truy cập.");
                }
                var resetToken = _jwtTokenGenerator.GenerateToken(user, roles);
                await SendResetPasswordEmail(email, resetToken);
                return true;
            }
            catch (Exception ex)
            {
                // Log exception here if you have a logging mechanism
                throw new Exception("An error occurred while processing ForgotPassword: " + ex.Message);
            }
        }

        private async Task SendResetPasswordEmail(string email, object resetToken)
        {
            try
            {
                var message = $"[Valid within {SD.ResetPassValidTime_Mins.ToString()} mins] Click the following link to reset your password: https://yourapp.com/reset-password?token={resetToken}";
                await _emailSender.SendEmailAsync(email, "Reset Password", message);
            }
            catch (Exception ex)
            {
                // Log exception here if you have a logging mechanism
                throw new Exception("An error occurred while sending the reset password email: " + ex.Message, ex);
            }
        }
        public async  Task<bool> ResetPassword(string resetToken, string newPass)
        {
            var user = _jwtTokenGenerator.ValidateToken(resetToken);
            if (user is not null)
            {
                var userFrmDb =  _unitOfWork.Users.GetUserById(user.Id);
                if (userFrmDb is not null)
                {                
                    var result = await _unitOfWork.Users.ResetPassword(userFrmDb, newPass);
                    if (result.Succeeded)
                        return true;

                }
            }
            return false;
        }
    }

 }
