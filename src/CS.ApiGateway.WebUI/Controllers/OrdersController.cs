using CS.ApiGateway.WebUI.ApiGateway.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CS.ApiGateway.WebUI.Controllers
{
    public class OrdersController(IApiGatewayOrderService orderService) : Controller
    {
        private readonly IApiGatewayOrderService orderService = orderService;


        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"))?.Value;

            return View(string.IsNullOrEmpty(userName) ? [] : await this.orderService.GetAllUserOrders(userName));
        }
        
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.orderService.GetOrderById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.orderService.GetOrderById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await this.orderService.GetOrderById(id);
            if (order != null)
            {
                await this.orderService.DeleteOrder(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
