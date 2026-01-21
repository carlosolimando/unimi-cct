using CS.ApiGateway.Core.Models;

namespace CS.ApiGateway.WebUI.ApiGateway.Users
{
    public interface IApiGatewayUserService
    {
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByUsername(string username);
        Task<IEnumerable<User>?> GetAllUsers();

        Task UpdateUser(User user);
        Task CreateUser(User user);
        Task DeleteUser(int id);

        Task<BasketItem?> GetBasketItemById(int id);
        Task<IEnumerable<BasketItem>?> GetAllBasketItems();

        Task UpdateBasketItem(BasketItem basketItem);
        Task CreateBasketItem(BasketItem basketItem);
        Task DeleteBasketItem(int id);

    }
}
