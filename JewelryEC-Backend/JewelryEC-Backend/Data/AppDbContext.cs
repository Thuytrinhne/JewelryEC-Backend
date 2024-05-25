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
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Models.Addresses;
using JewelryEC_Backend.Models.Deliveries;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Voucher;

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
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgresConstr"));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().Navigation(e => e.Items).AutoInclude();
            modelBuilder.Entity<Product>().HasMany(s => s.Items).WithOne(s => s.Product);
            modelBuilder.Entity<Product>().Navigation(e => e.Coupons).AutoInclude();
            modelBuilder.Entity<Product>().HasMany(s => s.Coupons).WithOne(s => s.Product);
            modelBuilder.Entity<Product>().Navigation(e => e.Catalog).AutoInclude();
            modelBuilder.Entity<Product>().HasOne(s => s.Catalog);

            modelBuilder.Entity<ProductVariant>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductCoupon>().Property(p => p.DiscountUnit)
                .HasConversion<string>();
            modelBuilder.Entity<ProductCoupon>().Navigation(p => p.Product).AutoInclude();

            modelBuilder.Entity<UserCoupon>().Navigation(u => u.ProductCoupon).AutoInclude();
            modelBuilder.Entity<UserCoupon>().Navigation(u => u.CouponApplications).AutoInclude();

            modelBuilder.Entity<UserCoupon>().Navigation(u => u.ProductCoupon).AutoInclude();
            modelBuilder.Entity<UserCoupon>().Navigation(u => u.CouponApplications).AutoInclude();

            modelBuilder.Entity<Order>().Navigation(o => o.OrderItems).AutoInclude();
            modelBuilder.Entity<CouponApplication>()
                .HasOne(ca => ca.OrderItem)
                .WithOne(oi => oi.CouponApplication)
                .HasForeignKey<CouponApplication>(ca => ca.OrderItemId)
                .IsRequired();

        }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CatalogCoupon> CatalogCoupons { get; set; }
        public DbSet<ProductCoupon> ProductCoupons { get; set; }
        public DbSet<UserCoupon> UserCoupons { get; set; }
        public DbSet<CouponApplication> CouponApplications { get; set; }    
    }
}
