namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; } // JWT token 

    }
}
