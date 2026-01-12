using CS.ApiGateway.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CS.ApiGateway.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly List<Product> Products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Category = ProductCategory.Meat,
                    Description = "desc",
                    Name = "name",
                    Price = 10
                },
                new Product
                {
                    Id = 2,
                    Category = ProductCategory.Vegetable,
                    Description = "desc",
                    Name = "name",
                    Price = 10
                },
                new Product
                {
                    Id = 3,
                    Category = ProductCategory.Bread,
                    Description = "desc",
                    Name = "name",
                    Price = 10
                },
                new Product
                {
                    Id = 4,
                    Category = ProductCategory.Drink,
                    Description = "desc",
                    Name = "name",
                    Price = 10
                } };
        public ProductsController()
        {
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(this.Products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this.Products.First(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

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
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Category,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                this.Products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this.Products.FirstOrDefault(x => x.Id == id);
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
                    this.Products[product.Id] = product;
                }
                catch (Exception)
                {
                    if (!ProductExists(product.Id))
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

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this.Products.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = this.Products.FirstOrDefault(Products => Products.Id == id);
            if (product != null)
            {
                this.Products.Remove(product);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return this.Products.Exists(p => p.Id == id);
        }
    }
}
