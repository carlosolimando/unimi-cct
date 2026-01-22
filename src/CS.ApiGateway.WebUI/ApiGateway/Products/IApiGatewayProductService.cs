using CS.ApiGateway.Core.Models;

namespace CS.ApiGateway.WebUI.ApiGateway.Products
{
    public interface IApiGatewayProductService
    {
        Task<Product?> GetProductById(int id);
        Task<IEnumerable<Product>?> GetAllProducts();

        Task UpdateProduct(Product product);
        Task CreateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
