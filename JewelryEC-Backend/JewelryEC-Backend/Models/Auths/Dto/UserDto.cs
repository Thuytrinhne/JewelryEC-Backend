namespace JewelryEC_Backend.Models.Auths.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; } = default!;
        public List<string> Roles { get; set; }


    }
}
