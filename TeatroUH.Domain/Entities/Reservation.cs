using System;
using System.Collections.Generic;

namespace TeatroUH.Domain.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Estado: Confirmed / Cancelled (por si luego lo ocupás)
        public string Status { get; set; } = "Confirmed";

        public decimal TotalAmount { get; set; }

        public List<ReservationItem> Items { get; set; } = new();
    }
}
