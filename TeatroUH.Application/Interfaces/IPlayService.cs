using TeatroUH.Domain.Entities;

namespace TeatroUH.Application.Interfaces
{
    public interface IPlayService
    {
        Task<List<Play>> GetAllAsync(bool onlyActive = true);
        Task<Play?> GetByIdAsync(int playId);
    }
}
