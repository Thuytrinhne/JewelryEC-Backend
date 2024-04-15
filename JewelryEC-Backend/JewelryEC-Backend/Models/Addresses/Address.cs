using JewelryEC_Backend.Core.Entity;

namespace JewelryEC_Backend.Models.Addresses
{
    public class Address: IEntity
    {
        public Guid Id { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public Guid UserId { get; set; }
    }
}
