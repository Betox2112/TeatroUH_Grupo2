using System.ComponentModel.DataAnnotations;

namespace TeatroUH.Web.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string CustomerName { get; set; } = "";

        [Required, EmailAddress]
        public string CustomerEmail { get; set; } = "";
    }
}
