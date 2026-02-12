using TeatroUH.Application.Models;

namespace TeatroUH.Application.Interfaces
{
    public interface ICartService
    {
        List<Cartitem> GetCart();
        void AddToCart(int showtimeId, int qty = 1);
        void RemoveFromCart(int showtimeId);
        void ClearCart();
        decimal GetTotal();
    }
}
