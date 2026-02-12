using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        public decimal TotalAmount { get; set; }

        // Navegación
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
