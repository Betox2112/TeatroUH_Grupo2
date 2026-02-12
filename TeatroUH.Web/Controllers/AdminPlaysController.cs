using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Domain.Entities;
using TeatroUH.Web.Security;

namespace TeatroUH.Web.Controllers
{
    [AdminOnly]
    public class AdminPlaysController : Controller
    {
        private readonly TeatroUHDbContext _db;

        public AdminPlaysController(TeatroUHDbContext db)
        {
            _db = db;
        }

        // GET: /AdminPlays
        public async Task<IActionResult> Index()
        {
            var plays = await _db.Plays
                .OrderByDescending(p => p.PlayId)
                .ToListAsync();

            return View(plays);
        }

        // GET: /AdminPlays/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Play { IsActive = true });
        }

        // POST: /AdminPlays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Play model, IFormFile? imageFile)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError(nameof(model.Title), "El título es requerido.");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Subir imagen (opcional)
                if (imageFile != null && imageFile.Length > 0)
                {
                    // 5MB máximo
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "La imagen es muy grande. Máximo 5MB.");
                        return View(model);
                    }

                    var imageUrl = await SavePlayImage(imageFile);
                    model.ImageUrl = imageUrl;
                }

                model.CreatedAt = DateTime.Now;

                _db.Plays.Add(model);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error subiendo imagen: {ex.Message}");
                return View(model);
            }
        }

        // GET: /AdminPlays/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var play = await _db.Plays.FindAsync(id);
            if (play == null) return NotFound();

            return View(play);
        }

        // POST: /AdminPlays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Play model, IFormFile? imageFile)
        {
            var play = await _db.Plays.FindAsync(id);
            if (play == null) return NotFound();

            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError(nameof(model.Title), "El título es requerido.");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Actualizar campos
                play.Title = model.Title;
                play.Description = model.Description;
                play.DurationMinutes = model.DurationMinutes;
                play.Rating = model.Rating;
                play.IsActive = model.IsActive;

                // Subir nueva imagen (si viene)
                if (imageFile != null && imageFile.Length > 0)
                {
                    // 5MB máximo
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "La imagen es muy grande. Máximo 5MB.");
                        return View(model);
                    }

                    var imageUrl = await SavePlayImage(imageFile);
                    play.ImageUrl = imageUrl;
                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error subiendo imagen: {ex.Message}");
                return View(model);
            }
        }

        // POST: /AdminPlays/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var play = await _db.Plays.FindAsync(id);
            if (play == null) return NotFound();

            _db.Plays.Remove(play);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminPlays/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var play = await _db.Plays.FindAsync(id);
            if (play == null) return NotFound();

            play.IsActive = !play.IsActive;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // Helper: Guardar imagen en AppData\Local\TeatroUH\uploads\plays
        // y devolver ruta pública: /uploads/plays/xxxx.png
        // =========================================================
        private async Task<string> SavePlayImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Archivo vacío.");

            // ✅ MISMA carpeta que Program.cs sirve como /uploads
            var uploadsRoot = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "TeatroUH",
                "uploads"
            );

            var playsDir = Path.Combine(uploadsRoot, "plays");
            Directory.CreateDirectory(playsDir);

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? "";
            var allowed = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowed.Contains(ext))
                throw new Exception("Formato inválido. Solo: jpg, jpeg, png, webp.");

            var safeName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(playsDir, safeName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/plays/{safeName}";
        }

    }
}
