using TeatroUH.Domain.Entities;

namespace TeatroUH.Application.Interfaces
{
    public interface IAuthService
    {
        User? Login(string email, string password);
        bool Register(User user);
    }
}
