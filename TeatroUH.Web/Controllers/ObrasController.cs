using Microsoft.AspNetCore.Mvc;
using TeatroUH.Application.Interfaces;

namespace TeatroUH.Web.Controllers
{
    public class ObrasController : Controller
    {
        private readonly IPlayService _playService;

        public ObrasController(IPlayService playService)
        {
            _playService = playService;
        }

        public async Task<IActionResult> Index()
        {
            var plays = await _playService.GetAllAsync(onlyActive: true);
            return View(plays);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var play = await _playService.GetByIdAsync(id);
            if (play == null) return NotFound();
            return View(play);
        }


    }
}
