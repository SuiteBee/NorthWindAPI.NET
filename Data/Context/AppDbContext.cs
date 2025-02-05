using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Resources;

namespace NorthWindAPI.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlite("SqliteConnection");
            }

            base.OnConfiguring(builder);
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Shipper> Shipper { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Auth> Auth { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
    }
}
