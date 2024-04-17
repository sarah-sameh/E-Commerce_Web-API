using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> products { set; get; }
        public DbSet<Category> categories { set; get; }
        public DbSet<Shipment> shipments { set; get; }
        public DbSet<Payment> payments { set; get; }
        public DbSet<Cart> carts { set; get; }

        public DbSet<WishList> wishLists { set; get; }
        public Context() : base()
        {

        }
        //constructor inject dbcontextOption
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product1", Price = 10, Description = "Description1" },
                new Product { Id = 2, Name = "Product2", Price = 20, Description = "Description2" }

            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
