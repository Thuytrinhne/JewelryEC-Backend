namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class RegistrationDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string OTP {  get; set; }
    }
}
