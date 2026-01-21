using CS.ApiGateway.Core.Models;

namespace CS.ApiGateway.WebUI.ApiGateway.Orders
{
    public class ApiGatewayOrderService(HttpClient httpClient) : IApiGatewayOrderService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task DeleteOrder(int id)
        {
            using var httpResponseMessage = await this.httpClient.DeleteAsync($"api/orders/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Order>?> GetAllUserOrders(string username)
        {
            var orders = await this.httpClient.GetFromJsonAsync<IEnumerable<Order>>("api/orders/");

            return orders?.Where(o => o.User.Equals(username));
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await this.httpClient.GetFromJsonAsync<Order>($"api/orders/{id}");
        }
    }
}
