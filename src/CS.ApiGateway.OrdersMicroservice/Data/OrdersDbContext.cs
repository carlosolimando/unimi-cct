using CS.ApiGateway.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CS.ApiGateway.OrdersMicroservice.Data
{
    public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<OrderLine> OrderLine { get; set; } = default!;
    }
}
