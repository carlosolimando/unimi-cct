using CS.ApiGateway.Core.Models;

namespace CS.ApiGateway.WebUI.ApiGateway.Orders
{
    public interface IApiGatewayOrderService
    {
        Task<Order?> GetOrderById(int id);

        Task<IEnumerable<Order>?> GetAllUserOrders(string username);

        Task DeleteOrder(int id);

    }
}
