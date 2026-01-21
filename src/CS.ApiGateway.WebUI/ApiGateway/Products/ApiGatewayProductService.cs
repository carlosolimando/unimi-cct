using CS.ApiGateway.Core.Models;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace CS.ApiGateway.WebUI.ApiGateway.Products
{
    public class ApiGatewayProductService(HttpClient httpClient) : IApiGatewayProductService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task CreateProduct(Product product)
        {
            var productJson = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PostAsync("api/products", productJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(int id)
        {
            using var httpResponseMessage = await this.httpClient.DeleteAsync($"api/products/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Product>?> GetAllProducts()
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<Product>>("api/products/");
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await this.httpClient.GetFromJsonAsync<Product>($"api/products/{id}");
        }

        public async Task UpdateProduct(Product product)
        {
            var productJson = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PutAsync($"api/products/{product.Id}", productJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}
