using Microsoft.AspNetCore.Mvc;

namespace CS.ApiGateway.WebUI.Controllers
{
    public class BasketItemsController : Controller
    {

        public BasketItemsController()
        {
        }

        /*
        // GET: BasketItems
        public async Task<IActionResult> Index()
        {
            var cSApiGatewayWebUIContext2 = _context.BasketItem.Include(b => b.User);
            return View(await cSApiGatewayWebUIContext2.ToListAsync());
        }

        // GET: BasketItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // GET: BasketItems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FirstName");
            return View();
        }

        // POST: BasketItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ProductId,ProductName,Quantity,Price")] BasketItem basketItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basketItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FirstName", basketItem.UserId);
            return View(basketItem);
        }

        // GET: BasketItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FirstName", basketItem.UserId);
            return View(basketItem);
        }

        // POST: BasketItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductId,ProductName,Quantity,Price")] BasketItem basketItem)
        {
            if (id != basketItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basketItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketItemExists(basketItem.Id))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FirstName", basketItem.UserId);
            return View(basketItem);
        }

        // GET: BasketItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var basketItem = await _context.BasketItem.FindAsync(id);
            if (basketItem != null)
            {
                _context.BasketItem.Remove(basketItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItem.Any(e => e.Id == id);
        }
        */
    }
}
