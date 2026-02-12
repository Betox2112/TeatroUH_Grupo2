using System.Collections.Generic;
using System.Threading.Tasks;
using TeatroUH.Domain.Entities;

namespace TeatroUH.Application.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsItem>> GetAllActiveAsync();
        Task<List<NewsItem>> GetAllAsync(); // admin
        Task<NewsItem?> GetByIdAsync(int id);

        Task<int> CreateAsync(NewsItem news);
        Task<bool> UpdateAsync(NewsItem news);
        Task<bool> DeleteAsync(int id);
        Task<bool> SetActiveAsync(int id, bool active);
    }
}
