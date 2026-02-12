using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class Play
    {
        public int PlayId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int DurationMinutes { get; set; }
        public string? Rating { get; set; }
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        // Navegación
        public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
