namespace JewelryEC_Backend.Models.Auths.Entities
{
    public class EmailVerification
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set;}
        public DateTime Created_at { get; set; } = DateTime.UtcNow;
        public DateTime Updated_at { get; set; } = DateTime.UtcNow;
    }
}
