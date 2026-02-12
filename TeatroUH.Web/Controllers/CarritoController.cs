using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.Interfaces;
using TeatroUH.Infrastructure.Data;
using TeatroUH.Web.Models;
using TeatroUH.Domain.Entities;

public class CarritoController : Controller
{
    private readonly ICartService _cart;
    private readonly TeatroUHDbContext _db;

    public CarritoController(ICartService cart, TeatroUHDbContext db)
    {
        _cart = cart;
        _db = db;
    }

    public IActionResult Index()
    {
        ViewBag.Total = _cart.GetTotal();
        return View(_cart.GetCart());
    }

    public IActionResult Confirmar()
    {
        // si el carrito está vacío, no dejés entrar
        if (!_cart.GetCart().Any())
            return RedirectToAction("Index");

        return View(new CheckoutViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Confirmar(CheckoutViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var cart = _cart.GetCart();
        if (cart == null || !cart.Any())
        {
            TempData["Msg"] = "Tu carrito está vacío.";
            return RedirectToAction("Index");
        }

        // Transacción: o todo se confirma o nada cambia
        using var tx = await _db.Database.BeginTransactionAsync();

        try
        {
            // 1) Cargar showtimes reales desde DB
            var showtimeIds = cart.Select(x => x.ShowtimeId).Distinct().ToList();

            var showtimes = await _db.Showtimes
                .Where(s => showtimeIds.Contains(s.ShowtimeId))
                .ToListAsync();

            // 2) Validar que existan y que tengan cupos
            foreach (var item in cart)
            {
                var s = showtimes.FirstOrDefault(x => x.ShowtimeId == item.ShowtimeId);
                if (s == null)
                {
                    ModelState.AddModelError("", "Una función del carrito ya no existe.");
                    await tx.RollbackAsync();
                    return View(vm);
                }

                if (!s.IsActive || s.Status != "Scheduled")
                {
                    ModelState.AddModelError("", "Una función del carrito no está disponible.");
                    await tx.RollbackAsync();
                    return View(vm);
                }

                if (s.SeatsAvailable < item.Quantity)
                {
                    ModelState.AddModelError("",
                        $"No hay cupos suficientes para una función. Disponibles: {s.SeatsAvailable}.");
                    await tx.RollbackAsync();
                    return View(vm);
                }
            }

            // 3) Crear la reserva
            var reservation = new Reservation
            {
                CustomerName = vm.CustomerName.Trim(),
                CustomerEmail = vm.CustomerEmail.Trim(),
                Status = "Confirmed",
                CreatedAt = DateTime.Now
            };

            // 4) Items + sumar total + actualizar SeatsSold
            decimal total = 0m;

            foreach (var item in cart)
            {
                var s = showtimes.First(x => x.ShowtimeId == item.ShowtimeId);

                // Precio final: usá lo del carrito o el BasePrice actual
                var unitPrice = s.BasePrice;

                var lineTotal = unitPrice * item.Quantity;
                total += lineTotal;

                reservation.Items.Add(new ReservationItem
                {
                    ShowtimeId = s.ShowtimeId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    LineTotal = lineTotal
                });

                // Restar cupos (en tu modelo: sumar vendidos)
                s.SeatsSold += item.Quantity;
            }

            reservation.TotalAmount = total;

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();

            // 5) Vaciar carrito y enviar a pantalla final
            _cart.ClearCart();

            return RedirectToAction("Confirmada", new { id = reservation.ReservationId });
        }
        catch
        {
            await tx.RollbackAsync();
            ModelState.AddModelError("", "Ocurrió un error al confirmar la reserva.");
            return View(vm);
        }
    }

    public async Task<IActionResult> Confirmada(int id)
    {
        var reservation = await _db.Reservations
            .Include(r => r.Items)
            .ThenInclude(i => i.Showtime)
            .ThenInclude(s => s.Play)
            .FirstOrDefaultAsync(r => r.ReservationId == id);

        if (reservation == null)
        {
            TempData["Msg"] = "Reserva no encontrada.";
            return RedirectToAction("Index");
        }

        return View(reservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int showtimeId, int qty = 1)
    {
        if (qty < 1) qty = 1;

        _cart.AddToCart(showtimeId, qty);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int showtimeId)
    {
        _cart.RemoveFromCart(showtimeId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        _cart.ClearCart();
        return RedirectToAction("Index");
    }


}
