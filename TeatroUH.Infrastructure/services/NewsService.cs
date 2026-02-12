using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Application.Interfaces;
using TeatroUH.Domain.Entities;
using TeatroUH.Infrastructure.Data;

namespace TeatroUH.Infrastructure.Services
{
    public class NewsService : INewsService
    {
        private readonly TeatroUHDbContext _db;

        public NewsService(TeatroUHDbContext db)
        {
            _db = db;
        }

        // Usuario/invitado: solo activas
        public async Task<List<NewsItem>> GetAllActiveAsync()
        {
            return await _db.NewsItems
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.PublishedAt)
                .ToListAsync();
        }

        // Admin: todas
        public async Task<List<NewsItem>> GetAllAsync()
        {
            return await _db.NewsItems
                .AsNoTracking()
                .OrderByDescending(x => x.PublishedAt)
                .ToListAsync();
        }

        public async Task<NewsItem?> GetByIdAsync(int id)
        {
            return await _db.NewsItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NewsItemId == id);
        }

        public async Task<int> CreateAsync(NewsItem news)
        {
            news.PublishedAt = news.PublishedAt == default ? DateTime.Now : news.PublishedAt;
            _db.NewsItems.Add(news);
            await _db.SaveChangesAsync();
            return news.NewsItemId;
        }

        public async Task<bool> UpdateAsync(NewsItem news)
        {
            var dbItem = await _db.NewsItems.FirstOrDefaultAsync(x => x.NewsItemId == news.NewsItemId);
            if (dbItem == null) return false;

            dbItem.Title = news.Title;
            dbItem.Summary = news.Summary;
            dbItem.Content = news.Content;
            dbItem.ImageUrl = news.ImageUrl;
            dbItem.PublishedAt = news.PublishedAt;
            dbItem.IsActive = news.IsActive;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dbItem = await _db.NewsItems.FirstOrDefaultAsync(x => x.NewsItemId == id);
            if (dbItem == null) return false;

            _db.NewsItems.Remove(dbItem);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetActiveAsync(int id, bool active)
        {
            var dbItem = await _db.NewsItems.FirstOrDefaultAsync(x => x.NewsItemId == id);
            if (dbItem == null) return false;

            dbItem.IsActive = active;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
