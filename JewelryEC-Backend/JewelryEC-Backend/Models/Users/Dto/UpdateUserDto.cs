namespace JewelryEC_Backend.Models.Users.Dto
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
