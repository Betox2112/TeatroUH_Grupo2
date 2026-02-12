using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Application.Cart
{
    public class CartItemDto
    {
        public int ShowtimeId { get; set; }
        public string PlayTitle { get; set; } = "";
        public DateTime Date { get; set; }
        public string Hall { get; set; } = "";   // sala/teatro
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
