using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public int ShowtimeId { get; set; }
        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navegación
        public Order? Order { get; set; }
    }
}
