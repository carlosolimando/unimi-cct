using CS.ApiGateway.ProductsMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace CS.ApiGateway.ProductsMicroservice.Data
{
    public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; } = default!;
    }
}
