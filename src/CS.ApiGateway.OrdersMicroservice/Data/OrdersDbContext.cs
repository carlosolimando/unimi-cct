using CS.ApiGateway.OrdersMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace CS.ApiGateway.OrdersMicroservice.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<OrderLine> OrderLine { get; set; } = default!;
    }
}
