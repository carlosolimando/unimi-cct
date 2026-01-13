namespace CS.ApiGateway.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }

    public class BasketItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public User User { get; set; } = null!;
    }
}
