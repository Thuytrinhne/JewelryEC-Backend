using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Carts.Entities;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Cart GetCartHeader(Guid userId);
        void SetStatusForCart(int status, Guid cartId);
    }
}
