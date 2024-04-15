using JewelryEC_Backend.Models.Addresses;

namespace JewelryEC_Backend.Models.Deliveries.Dto
{
    public class CreateDeliveryDto
    {
        public Guid UserId { get; set; }
        public Boolean IsDepartment { get; set; }
        public Boolean ReceiverIsMe { get; set; }
        public String Information { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
    }
}
