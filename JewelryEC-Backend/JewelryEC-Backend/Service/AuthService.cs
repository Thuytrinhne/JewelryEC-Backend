using AutoMapper;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Dto;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Catalogs.Dto;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace JewelryEC_Backend.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;


        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }



        public async Task<string> Register(RegistrationDto registrationDto)
        {
            ApplicationUser applicationUser = new()
            {
                UserName = registrationDto.Email,
                Email = registrationDto.Email,
                NormalizedEmail = registrationDto.Email.ToUpper(),
                Name = registrationDto.Name,
                PhoneNumber = registrationDto.PhoneNumber
            };
            try
            {
                var result = await  _userManager.CreateAsync(applicationUser, registrationDto.Password );
                if(result.Succeeded)
                {
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }    
            }
            catch
            {
                return "error";
            }
            
        }
        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginDto.Username.ToLower());

            bool isValid = await  _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDTO = _mapper.Map<UserDto>(user);

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;

        }
        public async Task<bool> AssignRole(string id, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
              
                
            }
            return false;

        }
    }
}
