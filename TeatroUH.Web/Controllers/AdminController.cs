using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Domain.Entities;
using TeatroUH.Web.Security;

namespace TeatroUH.Web.Controllers
{
    [AdminOnly]
    public class AdminController : Controller
    {
        private readonly TeatroUHDbContext _db;

        public AdminController(TeatroUHDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ✅ LISTA DE USUARIOS
        public async Task<IActionResult> Users()
        {
            var users = await _db.Users
                .OrderBy(u => u.Id)
                .ToListAsync();

            return View(users);
        }

        // ✅ HACER ADMIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Role = Role.Admin;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        // ✅ QUITAR ADMIN (volver a Comprador)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAdmin(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Para no dejarte sin admins por error, prevenimos quitarte a vos mismo
            var myEmail = HttpContext.Session.GetString("USER_EMAIL") ?? "";
            if (user.Email.Equals(myEmail, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MSG"] = "No podés quitarte el rol Admin a vos mismo.";
                return RedirectToAction(nameof(Users));
            }

            user.Role = Role.Comprador;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }
    }
}
