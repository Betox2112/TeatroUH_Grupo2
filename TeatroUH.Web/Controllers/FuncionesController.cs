using Microsoft.AspNetCore.Mvc;
using TeatroUH.Application.Interfaces;

namespace TeatroUH.Web.Controllers
{
    public class FuncionesController : Controller
    {
        private readonly IShowtimeService _showtimeService;

        public FuncionesController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        // ✅ Usuario: /Funciones  -> SOLO disponibles (activas, futuras, con cupo)
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = false;

            // Usuario no ve llenas/pasadas/inactivas
            var funciones = await _showtimeService.GetAvailableAsync();
            return View(funciones); // misma vista
        }

        // ✅ Usuario: /Funciones/PorObra?playId=1 -> SOLO disponibles por obra
        public async Task<IActionResult> PorObra(int playId)
        {
            ViewBag.IsAdmin = false;

            var funciones = await _showtimeService.GetAvailableByPlayIdAsync(playId);
            ViewBag.PlayId = playId;

            return View(funciones); // misma vista
        }

        // ✅ Usuario: detalle (si es admin puede ver cualquiera, si no, solo disponibles)
        public async Task<IActionResult> Detalle(int id)
        {
            // Para usuario normal, igual puede ver detalle si la función existe,
            // pero si querés bloquear detalle de funciones pasadas/inactivas,
            // lo hacemos con validación adicional (me decís).
            ViewBag.IsAdmin = false;

            var fn = await _showtimeService.GetByIdAsync(id);
            if (fn == null) return NotFound();

            return View(fn);
        }
    }
}
