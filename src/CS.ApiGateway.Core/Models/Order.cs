namespace CS.ApiGateway.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string User { get; set; }
        public required List<OrderLine> Products { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public int OrderId { get; set; }
        public required string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
