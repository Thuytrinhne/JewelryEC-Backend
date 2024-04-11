namespace JewelryEC_Backend.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public long AddressId { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Active { get; set; }
    }
}
