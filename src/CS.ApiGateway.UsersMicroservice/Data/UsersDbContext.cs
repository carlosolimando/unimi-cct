using CS.ApiGateway.UsersMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace CS.ApiGateway.UsersMicroservice.Data
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public DbSet<User> User { get; set; } = default!;
        public DbSet<BasketItem> BasketItem { get; set; } = default!;
    }
}
