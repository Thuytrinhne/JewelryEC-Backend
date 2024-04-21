using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Repository;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JewelryEC_Backend.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        
        public ICatalogRepository Catalogs { get; private set; }

        public ICartRepository Carts { get; private set; }

        public ICartItemRepository CartItems { get; private set; }

        public IEmailVerificationRespository EmailVerifications { get; private set; }

        public IUserRespository Users { get; private set; }

        public IRoleRespository Roles { get; private set; }
         public IOrderRepository Orders { get; private set; }
        public IProductRepository Products { get; private set; }
        public IShippingRepository Shippings { get; private set; }
        public IProductCouponRepository ProductCoupons { get; private set; }

        public UnitOfWork(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            Catalogs = new CatalogRepository(_context);
            Carts = new CartRepository(_context);
            CartItems = new CartItemRepository(_context);
            EmailVerifications = new EmailVerificationRespository(_context);
            Users = new UserRespository(_context, userManager, roleManager);
            Roles = new RoleRespository(_context, roleManager);
            Orders = new OrderRepository(_context);
            Products = new ProductRepository(_context);
            Shippings = new ShippingRepository(_context);
            ProductCoupons = new ProductCouponRepository(_context);

        }


        public int Save()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
