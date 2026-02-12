using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;
using TeatroUH.Infrastructure.Data;

namespace TeatroUH.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly TeatroUHDbContext _db;

        public AuthService(TeatroUHDbContext db)
        {
            _db = db;
        }

        public User? Login(string email, string password)
        {
            return _db.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool Register(User user)
        {
            if (_db.Users.Any(u => u.Email == user.Email))
                return false;

            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }
    }
}
