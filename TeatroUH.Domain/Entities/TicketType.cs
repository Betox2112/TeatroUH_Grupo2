using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class TicketType
    {
        public int TicketTypeId { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal PriceFactor { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
