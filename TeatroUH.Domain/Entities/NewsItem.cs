using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class NewsItem
    {
        public int NewsItemId { get; set; }

        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Content { get; set; } = "";
        public string? ImageUrl { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
