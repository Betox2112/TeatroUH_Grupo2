using System;
using System.Collections.Generic;
using System.Text;

namespace TeatroUH.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Comprador;
    }
}
