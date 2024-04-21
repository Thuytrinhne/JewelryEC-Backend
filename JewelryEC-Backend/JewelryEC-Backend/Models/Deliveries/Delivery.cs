using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Models.Addresses;
namespace JewelryEC_Backend.Models.Deliveries
{
    public class Delivery: IEntity
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        public Boolean IsDepartment { get; set; }
        public Boolean ReceiverIsMe { get; set; }
        public String Information { get; set; }
        public virtual Address Address { get; set; }
    }
}
