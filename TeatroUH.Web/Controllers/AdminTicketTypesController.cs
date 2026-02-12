using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Domain.Entities;
using TeatroUH.Web.Security;

namespace TeatroUH.Web.Controllers
{
    [AdminOnly]
    public class AdminTicketTypesController : Controller
    {
        private readonly TeatroUHDbContext _db;

        public AdminTicketTypesController(TeatroUHDbContext db)
        {
            _db = db;
        }

        // GET: /AdminTicketTypes
        public async Task<IActionResult> Index()
        {
            var items = await _db.TicketTypes
                .OrderByDescending(t => t.TicketTypeId)
                .ToListAsync();

            return View(items);
        }

        // GET: /AdminTicketTypes/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TicketType { IsActive = true, PriceFactor = 1.0000m });
        }

        // POST: /AdminTicketTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketType model)
        {
            ValidateTicketType(model);

            if (!ModelState.IsValid)
                return View(model);

            _db.TicketTypes.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminTicketTypes/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.TicketTypes.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: /AdminTicketTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketType model)
        {
            var item = await _db.TicketTypes.FindAsync(id);
            if (item == null) return NotFound();

            ValidateTicketType(model);

            if (!ModelState.IsValid)
                return View(model);

            item.Name = model.Name;
            item.PriceFactor = model.PriceFactor;
            item.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminTicketTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TicketTypes.FindAsync(id);
            if (item == null) return NotFound();

            _db.TicketTypes.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminTicketTypes/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var item = await _db.TicketTypes.FindAsync(id);
            if (item == null) return NotFound();

            item.IsActive = !item.IsActive;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // -------- Helpers --------
        private void ValidateTicketType(TicketType model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError(nameof(model.Name), "El nombre es requerido.");

            if (model.PriceFactor <= 0)
                ModelState.AddModelError(nameof(model.PriceFactor), "El factor debe ser mayor a 0.");
        }
    }
}
