using System;

namespace TeatroUH.Domain.Entities
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }

        // Relaciones
        public int PlayId { get; set; }
        public int TheaterId { get; set; }

        // Fecha y precio
        public DateTime StartDateTime { get; set; }
        public decimal BasePrice { get; set; }

        // Estado lógico (Scheduled, Cancelled, Finished, etc.)
        public string Status { get; set; } = "Scheduled";

        // Control manual de visibilidad
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navegación
        public Play? Play { get; set; }
        public Theater? Theater { get; set; }

        // Control de asientos
        public int Capacity { get; set; } = 1500;
        public int SeatsSold { get; set; } = 0;

        // Propiedades calculadas
        public int SeatsAvailable => Capacity - SeatsSold;
        public bool IsFull => SeatsAvailable <= 0;
    }
}
