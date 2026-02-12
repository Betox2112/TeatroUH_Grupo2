using System;

namespace TeatroUH.Domain.Entities
{
    public class ReservationItem
    {
        public int ReservationItemId { get; set; }

        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int ShowtimeId { get; set; }
        public Showtime? Showtime { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
