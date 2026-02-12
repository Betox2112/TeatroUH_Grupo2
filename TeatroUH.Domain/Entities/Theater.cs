using System;
using System.Collections.Generic;

namespace TeatroUH.Domain.Entities
{
    public class Theater
    {
        public int TheaterId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }

        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navegación
        public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
