using CS.ApiGateway.Core.Models;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace CS.ApiGateway.WebUI.ApiGateway.Users
{
    public class ApiGatewayUserService(HttpClient httpClient) : IApiGatewayUserService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task CreateUser(User user)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PostAsync("api/users", userJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task DeleteUser(int id)
        {
            using var httpResponseMessage = await this.httpClient.DeleteAsync($"api/users/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<User>?> GetAllUsers()
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<User>>("api/users/");
        }

        public async Task<User?> GetUserById(int id)
        {
            return await this.httpClient.GetFromJsonAsync<User>($"api/users/{id}");
        }

        public async Task UpdateUser(User user)
        {
            var userJson = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PutAsync($"api/users/{user.Id}", userJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task CreateBasketItem(BasketItem basketitem)
        {
            var basketitemJson = new StringContent(JsonSerializer.Serialize(basketitem), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PostAsync("api/basketitems", basketitemJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task DeleteBasketItem(int id)
        {
            using var httpResponseMessage = await this.httpClient.DeleteAsync($"api/basketitems/{id}");

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<BasketItem>?> GetAllBasketItems()
        {
            return await this.httpClient.GetFromJsonAsync<IEnumerable<BasketItem>>("api/basketitems/");
        }

        public async Task<BasketItem?> GetBasketItemById(int id)
        {
            return await this.httpClient.GetFromJsonAsync<BasketItem>($"api/basketitems/{id}");
        }

        public async Task UpdateBasketItem(BasketItem basketitem)
        {
            var basketitemJson = new StringContent(JsonSerializer.Serialize(basketitem), Encoding.UTF8, Application.Json);

            using var httpResponseMessage = await this.httpClient.PutAsync($"api/basketitems/{basketitem.Id}", basketitemJson);

            httpResponseMessage.EnsureSuccessStatusCode();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var users = await this.GetAllUsers();

            return users?.FirstOrDefault(x => x.UserName.Equals(username));
        }
    }
}
