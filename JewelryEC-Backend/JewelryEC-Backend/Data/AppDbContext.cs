using JewelryEC_Backend.Models.Catalogs.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Roles.Entities;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Models.Addresses;
using JewelryEC_Backend.Models.Deliveries;
using JewelryEC_Backend.Models.Coupon;

namespace JewelryEC_Backend.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        protected readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CatalogCoupon> CatalogCoupons { get; set; }
        public DbSet<ProductCoupon> ProductCoupons { get; set; }
    }
}
