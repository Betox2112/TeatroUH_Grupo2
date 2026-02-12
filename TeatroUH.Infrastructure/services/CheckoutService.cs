using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;
using TeatroUH.Infrastructure.Data;

namespace TeatroUH.Infrastructure.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly TeatroUHDbContext _db;
        private readonly ICartService _cart;

        public CheckoutService(TeatroUHDbContext db, ICartService cart)
        {
            _db = db;
            _cart = cart;
        }

        // ✅ Crea la orden, descuenta cupos (SeatsSold) y vacía carrito
        public int CreateOrder(string customerName, string customerEmail)
        {
            // Si querés 100% async, abajo te dejo versión async opcional,
            // pero esta compila perfecto y respeta tu firma int.
            var cartItems = _cart.GetCart();
            if (cartItems == null || !cartItems.Any()) return 0;

            var now = DateTime.Now;

            // ✅ Validaciones: showtime existe, no está en el pasado, y hay cupo
            foreach (var item in cartItems)
            {
                var showtime = _db.Showtimes
                    .AsNoTracking()
                    .FirstOrDefault(s => s.ShowtimeId == item.ShowtimeId);

                if (showtime == null) return 0;
                if (showtime.StartDateTime <= now) return 0;

                var available = showtime.Capacity - showtime.SeatsSold;
                if (available < item.Quantity) return 0;
            }

            using var tx = _db.Database.BeginTransaction();

            try
            {
                // ✅ Crear orden (Pago en efectivo)
                var order = new Order
                {
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    OrderDate = DateTime.Now,
                    PaymentStatus = "Cash", // "Paid" no aplica si es efectivo y se paga al llegar
                    TotalAmount = cartItems.Sum(x => x.UnitPrice * x.Quantity),
                    Items = new List<OrderItem>()
                };

                _db.Orders.Add(order);
                _db.SaveChanges(); // para obtener OrderId

                // ✅ OrderItems + actualizar SeatsSold
                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ShowtimeId = item.ShowtimeId,
                        TicketTypeId = 1, // luego lo conectamos bien con Adulto/Niño/AdultoMayor
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };

                    _db.OrderItems.Add(orderItem);

                    var showtime = _db.Showtimes.First(s => s.ShowtimeId == item.ShowtimeId);
                    showtime.SeatsSold += item.Quantity;
                }

                _db.SaveChanges();

                tx.Commit();

                _cart.ClearCart();
                return order.OrderId;
            }
            catch
            {
                tx.Rollback();
                return 0;
            }
        }

        // ✅ Este es el que te faltaba (para que no salga CS0535)
        public async Task<bool> ConfirmPurchaseAsync()
        {
            // Como tu flujo real hoy confirma en CreateOrder,
            // esto queda como confirmación "ok" del proceso.
            // Más adelante podemos mover la lógica aquí si querés 2 pasos.
            await Task.CompletedTask;
            return true;
        }
    }
}
