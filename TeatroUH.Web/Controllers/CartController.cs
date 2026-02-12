using Microsoft.AspNetCore.Mvc;
using TeatroUH.Application.Interfaces;
/*
namespace TeatroUH.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cart)
        {
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart.GetCart());
        }

        [HttpPost]
        public IActionResult Add(int showtimeId, int qty = 1)
        {
            _cart.AddToCart(showtimeId, qty);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int showtimeId)
        {
            _cart.RemoveFromCart(showtimeId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _cart.ClearCart();
            return RedirectToAction("Index");
        }
    }
}
*/