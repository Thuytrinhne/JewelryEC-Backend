namespace JewelryEC_Backend.Models.Users.Dto
{
    public class GetUserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarUrl { get; set; } = default!;
        public List<string> Roles { get; set; }


    }
}
