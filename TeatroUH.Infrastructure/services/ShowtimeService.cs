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
    public class ShowtimeService : IShowtimeService
    {
        private readonly TeatroUHDbContext _db;

        public ShowtimeService(TeatroUHDbContext db)
        {
            _db = db;
        }

        // ✅ ADMIN: Devuelve TODO (incluye pasadas/llenas/inactivas)
        public async Task<List<Showtime>> GetAllAsync()
        {
            return await _db.Showtimes
                .AsNoTracking()
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        // ✅ ADMIN: Por obra (sin filtrar)
        public async Task<List<Showtime>> GetByPlayIdAsync(int playId)
        {
            return await _db.Showtimes
                .AsNoTracking()
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .Where(s => s.PlayId == playId)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        // ✅ Obtener una función por ID
        public async Task<Showtime?> GetByIdAsync(int id)
        {
            return await _db.Showtimes
                .AsNoTracking()
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);
        }

        // =========================
        // ✅ USUARIO: SOLO DISPONIBLES
        // =========================

        // ✅ Disponibles: activas, futuras, con cupo
        public async Task<List<Showtime>> GetAvailableAsync()
        {
            var now = DateTime.Now;

            return await _db.Showtimes
                .AsNoTracking()
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .Where(s => s.IsActive
                         && s.StartDateTime > now
                         && s.SeatsSold < s.Capacity)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        // ✅ Disponibles por obra
        public async Task<List<Showtime>> GetAvailableByPlayIdAsync(int playId)
        {
            var now = DateTime.Now;

            return await _db.Showtimes
                .AsNoTracking()
                .Include(s => s.Play)
                .Include(s => s.Theater)
                .Where(s => s.PlayId == playId
                         && s.IsActive
                         && s.StartDateTime > now
                         && s.SeatsSold < s.Capacity)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();
        }

        // ✅ ADMIN: activar / desactivar función
        public async Task<bool> SetActiveAsync(int id, bool active)
        {
            var fn = await _db.Showtimes
                .FirstOrDefaultAsync(s => s.ShowtimeId == id);

            if (fn == null) return false;

            fn.IsActive = active;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
