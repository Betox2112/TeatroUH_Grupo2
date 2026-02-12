using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Domain.Entities;
using TeatroUH.Web.Security;

namespace TeatroUH.Web.Controllers
{
    [AdminOnly]
    public class AdminTheatersController : Controller
    {
        private readonly TeatroUHDbContext _db;

        public AdminTheatersController(TeatroUHDbContext db)
        {
            _db = db;
        }

        // GET: /AdminTheaters
        public async Task<IActionResult> Index()
        {
            var theaters = await _db.Theaters
                .OrderByDescending(t => t.TheaterId)
                .ToListAsync();

            return View(theaters);
        }

        // GET: /AdminTheaters/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Theater { IsActive = true });
        }

        // POST: /AdminTheaters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Theater model)
        {
            ValidateTheater(model);

            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;

            _db.Theaters.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminTheaters/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var theater = await _db.Theaters.FindAsync(id);
            if (theater == null) return NotFound();

            return View(theater);
        }

        // POST: /AdminTheaters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Theater model)
        {
            var theater = await _db.Theaters.FindAsync(id);
            if (theater == null) return NotFound();

            ValidateTheater(model);

            if (!ModelState.IsValid)
                return View(model);

            theater.Name = model.Name;
            theater.Location = model.Location;
            theater.Capacity = model.Capacity;
            theater.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminTheaters/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var theater = await _db.Theaters.FindAsync(id);
            if (theater == null) return NotFound();

            _db.Theaters.Remove(theater);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminTheaters/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var theater = await _db.Theaters.FindAsync(id);
            if (theater == null) return NotFound();

            theater.IsActive = !theater.IsActive;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // -------- Helpers --------
        private void ValidateTheater(Theater model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError(nameof(model.Name), "El nombre es requerido.");

            // Location es nullable, pero si querés exigirla:
            // if (string.IsNullOrWhiteSpace(model.Location))
            //     ModelState.AddModelError(nameof(model.Location), "La ubicación es requerida.");

            if (model.Capacity <= 0)
                ModelState.AddModelError(nameof(model.Capacity), "La capacidad debe ser mayor a 0.");
        }
    }
}
