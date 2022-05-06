using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using demo_webshop.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace demo_webshop.Data
{
    // Ručno dodana klasa za ubacivanje prilagođenih svojstava u tablicu AspNetUsers
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(150)]
        public string Address { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Dodatno podešavanje ograničenja između OrderItem i Order
            builder
                .Entity<OrderItem>()
                .HasOne<Order>(o => o.Order)
                .WithMany(oi => oi.Items)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Dodatno ograničenje između OrderItem i Product
            builder
                .Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany(oi => oi.Items)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}