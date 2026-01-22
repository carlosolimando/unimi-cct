using CS.ApiGateway.Core.Models;
using CS.ApiGateway.WebUI.ApiGateway.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CS.ApiGateway.WebUI.Controllers
{
    public class BasketItemsController(IApiGatewayUserService userService) : Controller
    {
        private readonly IApiGatewayUserService userService = userService;

        // GET: BasketItems
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"))?.Value;

            var user = string.IsNullOrWhiteSpace(userName) ? null : await this.userService.GetUserByUsername(userName);

            if (user == null)
            {
                return NotFound();
            }

            var basketItems = await this.userService.GetAllBasketItems();

            return View(string.IsNullOrWhiteSpace(userName) || basketItems == null ? [] : basketItems.Where(x => x.UserId == user.Id));
        }



        // GET: BasketItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await this.userService.GetBasketItemById(id.Value);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // GET: BasketItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await this.userService.GetBasketItemById(id.Value);
            if (basketItem == null)
            {
                return NotFound();
            }
            return View(basketItem);
        }

        // POST: BasketItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,ProductName,Quantity,Price,UserId")] BasketItem basketItem)
        {
            var basketItem2 = await this.userService.GetBasketItemById(basketItem.Id);

            if (id != basketItem.Id || basketItem2 == null)
            {
                return NotFound();
            }

            try
            {
                basketItem2.Quantity = basketItem.Quantity;
                await this.userService.UpdateBasketItem(basketItem2);
            }
            catch (Exception)
            {

                if (basketItem2 == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }



        // GET: BasketItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await this.userService.GetBasketItemById(id.Value);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // POST: BasketItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var backetItem = await this.userService.GetBasketItemById(id);
            if (backetItem != null)
            {
                await this.userService.DeleteBasketItem(id);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Purchase
        public async Task<IActionResult> Purchase()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"))?.Value;

            var user = string.IsNullOrWhiteSpace(userName) ? null : await this.userService.GetUserByUsername(userName);

            if (user == null)
            {
                return NotFound();
            }

            try
            {

                var basketItems = await this.userService.GetAllBasketItems();

                if (string.IsNullOrWhiteSpace(userName) || basketItems == null)
                    return NotFound();

                user.BasketItems = [.. basketItems.Where(x => x.UserId == user.Id)];

                await this.userService.UpdateUser(user);

                foreach (var basketItem in basketItems)
                {
                    await this.userService.DeleteBasketItem(basketItem.Id);
                }
            }
            catch (Exception)
            {
                var user2 = await this.userService.GetUserById(user.Id);

                if (user2 == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Orders");

        }
    }
}
