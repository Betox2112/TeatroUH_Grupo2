using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;
using TeatroUH.Infrastructure.Data;

namespace TeatroUH.Infrastructure.Services
{
    public class PlayService : IPlayService
    {
        private readonly TeatroUHDbContext _db;

        public PlayService(TeatroUHDbContext db)
        {
            _db = db;
        }

        public async Task<List<Play>> GetAllAsync(bool onlyActive = true)
        {
            var query = _db.Plays.AsQueryable();

            if (onlyActive)
                query = query.Where(p => p.IsActive);

            return await query
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<Play?> GetByIdAsync(int playId)
        {
            return await _db.Plays.FirstOrDefaultAsync(p => p.PlayId == playId);
        }
    }
}
