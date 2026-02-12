using Microsoft.AspNetCore.Mvc;
using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;

namespace TeatroUH.Web.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly INewsService _news;

        public NoticiasController(INewsService news)
        {
            _news = news;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("USER_ROLE");
            return role == "Admin";
        }

        // GET: /Noticias
        public async Task<IActionResult> Index()
        {
            var items = IsAdmin()
                ? await _news.GetAllAsync()
                : await _news.GetAllActiveAsync();

            ViewBag.IsAdmin = IsAdmin();
            return View(items);
        }

        // GET: /Noticias/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var item = await _news.GetByIdAsync(id);
            if (item == null) return NotFound();

            // Si no es admin, no dejar ver inactivas
            if (!IsAdmin() && !item.IsActive) return NotFound();

            ViewBag.IsAdmin = IsAdmin();
            return View(item);
        }

        // ======================
        // ADMIN CRUD
        // ======================

        public IActionResult Crear()
        {
            if (!IsAdmin()) return View("AccessDenied");
            return View(new NewsItem { PublishedAt = DateTime.Now, IsActive = true });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(NewsItem model)
        {
            if (!IsAdmin()) return View("AccessDenied");

            if (string.IsNullOrWhiteSpace(model.Title))
                ModelState.AddModelError("", "El título es obligatorio.");

            if (!ModelState.IsValid) return View(model);

            await _news.CreateAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (!IsAdmin()) return View("AccessDenied");

            var item = await _news.GetByIdAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(NewsItem model)
        {
            if (!IsAdmin()) return View("AccessDenied");

            if (!ModelState.IsValid) return View(model);

            var ok = await _news.UpdateAsync(model);
            if (!ok) return NotFound();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, bool active)
        {
            if (!IsAdmin()) return View("AccessDenied");

            await _news.SetActiveAsync(id, active);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!IsAdmin()) return View("AccessDenied");

            await _news.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
