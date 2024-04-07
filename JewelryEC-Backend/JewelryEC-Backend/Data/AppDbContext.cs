using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Products;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace JewelryEC_Backend.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Product> Products { get; set; }



    }
}
