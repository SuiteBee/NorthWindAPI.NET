using Microsoft.EntityFrameworkCore;
using NorthWindAPI.Data.Models;

namespace NorthWindAPI.Config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
