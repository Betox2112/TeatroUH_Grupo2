using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Domain.Entities;
using TeatroUH.Web.Security;

namespace TeatroUH.Web.Controllers
{
    [AdminOnly]
    public class AdminShowtimesController : Controller
    {
        private readonly TeatroUHDbContext _db;

        public AdminShowtimesController(TeatroUHDbContext db)
        {
            _db = db;
        }

        // GET: /AdminShowtimes
        public async Task<IActionResult> Index()
        {
            var items = await _db.Showtimes
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .OrderByDescending(s => s.StartDateTime)
                .ToListAsync();

            return View(items);
        }

        // GET: /AdminShowtimes/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCombos();
            return View(new Showtime
            {
                StartDateTime = DateTime.Now.AddDays(1),
                BasePrice = 3000,
                Status = "Scheduled",
                IsActive = true
            });
        }

        // POST: /AdminShowtimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Showtime model)
        {
            ValidateShowtime(model);

            if (!ModelState.IsValid)
            {
                await LoadCombos();
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            model.SeatsSold = 0; // por si acaso

            _db.Showtimes.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminShowtimes/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var showtime = await _db.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();

            await LoadCombos();
            return View(showtime);
        }

        // POST: /AdminShowtimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Showtime model)
        {
            var showtime = await _db.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();

            // Mantener SeatsSold tal cual está (no lo edites manual)
            var seatsSoldActual = showtime.SeatsSold;

            ValidateShowtime(model);

            if (!ModelState.IsValid)
            {
                await LoadCombos();
                model.SeatsSold = seatsSoldActual;
                return View(model);
            }

            showtime.PlayId = model.PlayId;
            showtime.TheaterId = model.TheaterId;
            showtime.StartDateTime = model.StartDateTime;
            showtime.BasePrice = model.BasePrice;
            showtime.Status = model.Status;
            showtime.IsActive = model.IsActive;

            // Capacidad: si la cambiás, no puede quedar menor que SeatsSold
            showtime.Capacity = model.Capacity < seatsSoldActual ? seatsSoldActual : model.Capacity;
            showtime.SeatsSold = seatsSoldActual;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminShowtimes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var showtime = await _db.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();

            _db.Showtimes.Remove(showtime);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminShowtimes/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var showtime = await _db.Showtimes.FindAsync(id);
            if (showtime == null) return NotFound();

            showtime.IsActive = !showtime.IsActive;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // -------- Helpers --------

        private async Task LoadCombos()
        {
            ViewBag.Plays = await _db.Plays
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();

            ViewBag.Theaters = await _db.Theaters
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        private void ValidateShowtime(Showtime model)
        {
            if (model.PlayId <= 0)
                ModelState.AddModelError(nameof(model.PlayId), "Seleccione una obra.");

            if (model.TheaterId <= 0)
                ModelState.AddModelError(nameof(model.TheaterId), "Seleccione un teatro.");

            if (model.StartDateTime == default)
                ModelState.AddModelError(nameof(model.StartDateTime), "La fecha/hora es requerida.");

            if (model.BasePrice <= 0)
                ModelState.AddModelError(nameof(model.BasePrice), "El precio base debe ser mayor a 0.");

            if (model.Capacity <= 0)
                ModelState.AddModelError(nameof(model.Capacity), "La capacidad debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(model.Status))
                ModelState.AddModelError(nameof(model.Status), "El estado es requerido.");
        }
    }
}
