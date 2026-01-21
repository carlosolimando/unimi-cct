using CS.ApiGateway.Core.Models;
using CS.ApiGateway.WebUI.ApiGateway.Products;
using CS.ApiGateway.WebUI.ApiGateway.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CS.ApiGateway.WebUI.Controllers
{
    public class ProductsController(IApiGatewayProductService productService, IApiGatewayUserService userService) : Controller
    {
        private readonly IApiGatewayProductService productService = productService;
        private readonly IApiGatewayUserService userService = userService;

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var productList = await this.productService.GetAllProducts();
            return View(productList);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Policy = "AdminUserType")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminUserType")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Category,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                await this.productService.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Policy = "AdminUserType")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminUserType")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Category,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.productService.UpdateProduct(product);
                }
                catch (Exception)
                {
                    var product2 = await this.productService.GetProductById(product.Id);

                    if (product2 == null)
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
            return View(product);
        }

        //AddToBasket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToBasket([Bind("Id,Name,Description,Category,Price")] Product product)
        {
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"))?.Value;

            var user = string.IsNullOrWhiteSpace(userName) ? null : await this.userService.GetUserByUsername(userName);

            if (user == null || product == null || !product.Price.HasValue || string.IsNullOrWhiteSpace(product.Name)) {
                return NotFound();
            }

            var basketItem = new BasketItem
            {
                Price = product.Price.Value,
                ProductName = product.Name,
                ProductId = product.Id,
                Quantity = 1,
                UserId = user.Id
            };

            await this.userService.CreateBasketItem(basketItem);

            return RedirectToAction("Index", "BasketItems");

        }


        // GET: Products/Delete/5
        [Authorize(Policy = "AdminUserType")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminUserType")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productService.GetProductById(id);
            if (product != null)
            {
                await this.productService.DeleteProduct(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
