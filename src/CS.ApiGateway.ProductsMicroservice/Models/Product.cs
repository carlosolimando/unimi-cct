namespace CS.ApiGateway.ProductsMicroservice.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ProductCategory Category { get; set; }
        public decimal? Price { get; set; }
    }

    public enum ProductCategory
    {
        Bread,
        Meat,
        Vegetable,
        Drink
    }
}
