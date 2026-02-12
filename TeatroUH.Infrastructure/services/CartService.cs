using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.Interfaces;
using TeatroUH.Application.Models;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Infrastructure.Helpers;

namespace TeatroUH.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private const string CART_KEY = "CART";

        private readonly TeatroUHDbContext _db;
        private readonly IHttpContextAccessor _http;

        public CartService(TeatroUHDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        private ISession Session => _http.HttpContext!.Session;

        public List<Cartitem> GetCart()
        {
            return Session.GetObject<List<Cartitem>>(CART_KEY) ?? new List<Cartitem>();
        }

        public void AddToCart(int showtimeId, int qty = 1)
        {
            if (qty <= 0) qty = 1;

            var showtime = _db.Showtimes
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null) return;

            var cart = GetCart();

            var existing = cart.FirstOrDefault(x => x.ShowtimeId == showtimeId);
            if (existing != null)
            {
                existing.Quantity += qty;
            }
            else
            {
                cart.Add(new Cartitem
                {
                    ShowtimeId = showtime.ShowtimeId,
                    PlayTitle = showtime.Play?.Title ?? "Obra",
                    TheaterName = showtime.Theater?.Name ?? "Sala",
                    StartDateTime = showtime.StartDateTime,
                    UnitPrice = showtime.BasePrice,
                    Quantity = qty
                });
            }

            Session.SetObject(CART_KEY, cart);
        }

        public void RemoveFromCart(int showtimeId)
        {
            var cart = GetCart();
            cart.RemoveAll(x => x.ShowtimeId == showtimeId);
            Session.SetObject(CART_KEY, cart);
        }

        public void ClearCart()
        {
            Session.Remove(CART_KEY);
        }

        public decimal GetTotal()
        {
            var cart = GetCart();
            return cart.Sum(x => x.UnitPrice * x.Quantity);
        }
    }
}
