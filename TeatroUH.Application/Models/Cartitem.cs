using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Application.Models
{
    public class Cartitem
    {
        public int ShowtimeId { get; set; }
        public string PlayTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
