using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace JewelryEC_Backend.Service
{
    public class AuthService : IAuthService
    {
    
        private IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IService.IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork; 

        public AuthService(
            IMapper mapper, IJwtTokenGenerator jwtTokenGenerator,
            IService.IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
          
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }



        public async Task<string> Register(RegistrationDto registrationDto)
        {

            if (checkOTP(registrationDto.Email, registrationDto.OTP))
            {

                ApplicationUser applicationUser = new()
                {
                    UserName = registrationDto.Email.ToLower(),
                    Email = registrationDto.Email,
                    NormalizedEmail = registrationDto.Email.ToUpper(),
                    Name = registrationDto.Name,
                    PhoneNumber = registrationDto.PhoneNumber
                };
                try
                {
                    var result =   _unitOfWork.Users.AddUserByUserManager(applicationUser, registrationDto.Password);
                    if (result.Result.Succeeded)
                    {
                        return "";
                    }
                    else
                    {
                        return result.Result.Errors.FirstOrDefault().Description;
                    }
                }
                catch
                {
                    return "error";
                }

            }
            else
                return "otp not valid";
            
           
            
        }

        private bool checkOTP(string email, string otp)
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
            if (created_at.AddMinutes(10) >= currentTime)
            {
                return true;
            }
            return false;
        }
        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = await  _unitOfWork.Users.GetUserByEmail(loginDto.Email);
            if(user != null)
            {
                bool isValid = await _unitOfWork.Users.Login(user, loginDto.Password);
                if ( isValid == true)
                {
                    //if user was found , Generate JWT Token
                    var roles = await _unitOfWork.Users.GetRoleAsync(user);
                    var token = _jwtTokenGenerator.GenerateToken(user, roles);

                    UserDto userDTO = _mapper.Map<UserDto>(user);

                    LoginResponseDto loginResponseDto = new LoginResponseDto()
                    {
                        User = userDTO,
                        Token = token
                    };
                    return loginResponseDto;
                }

            }
            return new LoginResponseDto() { User = null, Token = "" };

        }
        public async Task<bool> AssignRole(Guid userId, string roleName)
        {
            var user = _unitOfWork.Users.GetUserById(userId);
            if (user != null)
            {
             if ( await  _unitOfWork.Users.AssignRoleForUser(user, roleName))
              return true;
            }
            return false;

        }

        public async Task<bool> SendingOTP(string email)
        {
            try
            {
                // send otp 
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();
                string subject = "Sending OTP";
                string message = "Your OTP is " + otp;
                await _emailSender.SendEmailAsync(email, subject, message);
                // insert email verifiaction 
                EmailVerification emailVerification = new EmailVerification();
                emailVerification.Id = new Guid();
                emailVerification.Email = email;
                emailVerification.Otp = otp;

                _unitOfWork.EmailVerifications.Add(emailVerification);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async  Task<bool> ForgotPassword(string email)
        {
            var user = await   _unitOfWork.Users.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }
            var roles = await _unitOfWork.Users.GetRoleAsync(user);
            var resetToken = _jwtTokenGenerator.GenerateToken(user, roles);
            SendResetPasswordEmail(email, resetToken);
            return true;
            
           
        }

        private async void SendResetPasswordEmail(string email, object resetToken)
        {
            var message = $"Click the following link to reset your password: https://yourapp.com/reset-password?token={resetToken}";
            await _emailSender.SendEmailAsync(email, "Reset Password", message);
        }
        public async  Task<bool> ResetPassword(string resetToken, string newPass)
        {
            var user = _jwtTokenGenerator.ValidateToken(resetToken);
            if (user != null)
            {

                var userFrmDb =  _unitOfWork.Users.GetUserById(user.Id);
                if (userFrmDb != null)
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
