using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {

        }

        public Cart GetCartHeader(Guid userId)
        {
            return Find(c => c.UserId == userId).FirstOrDefault();
            
        }

       
    }
}
