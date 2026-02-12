using TeatroUH.Domain.Entities;

namespace TeatroUH.Application.Interfaces
{
    public interface IShowtimeService
    {
        Task<List<Showtime>> GetAllAsync();
        Task<List<Showtime>> GetByPlayIdAsync(int playId);

        Task<Showtime?> GetByIdAsync(int id);
        Task<List<Showtime>> GetAvailableAsync();
        Task<List<Showtime>> GetAvailableByPlayIdAsync(int playId);
        Task<bool> SetActiveAsync(int id, bool active);


    }
}
