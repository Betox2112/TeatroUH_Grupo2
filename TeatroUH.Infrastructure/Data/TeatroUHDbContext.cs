
using TeatroUH.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using TeatroUH.Domain.Entities;

namespace TeatroUH.Infrastructure.Data
{
    public class TeatroUHDbContext : DbContext
    {
        public TeatroUHDbContext(DbContextOptions<TeatroUHDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Play> Plays { get; set; } = default!;
        public DbSet<Showtime> Showtimes { get; set; } = default!;
        public DbSet<Theater> Theaters { get; set; } = default!;
        public DbSet<TicketType> TicketTypes { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;
        public DbSet<NewsItem> NewsItems { get; set; } = default!;
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // ✅ Precisión decimal (quita warnings)
            // =========================
            modelBuilder.Entity<Order>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Showtime>()
                .Property(x => x.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TicketType>()
                .Property(x => x.PriceFactor)
                .HasPrecision(18, 4);

            // =========================
            // ✅ Defaults / reglas
            // =========================

            // Showtime.IsActive => DEFAULT 1
            modelBuilder.Entity<Showtime>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            // NewsItem.IsActive => DEFAULT 1
            modelBuilder.Entity<NewsItem>()
                .Property(x => x.IsActive)
                .HasDefaultValue(true);

            // NewsItem.PublishedAt => fecha actual (SQL)
            // Nota: esto crea DEFAULT(GETDATE()) para nuevos registros desde BD.
            modelBuilder.Entity<NewsItem>()
                .Property(x => x.PublishedAt)
                .HasDefaultValueSql("GETDATE()");

            // =========================
            // 📏 Longitudes de texto
            // =========================
            modelBuilder.Entity<Theater>()
                .Property(x => x.Name)
                .HasMaxLength(200);

            modelBuilder.Entity<Theater>()
                .Property(x => x.Location)
                .HasMaxLength(200);

            modelBuilder.Entity<Play>()
                .Property(x => x.Title)
                .HasMaxLength(200);

            modelBuilder.Entity<Play>()
                .Property(x => x.Description)
                .HasMaxLength(600);

            modelBuilder.Entity<Play>()
                .Property(x => x.ImageUrl)
                .HasMaxLength(500);

            modelBuilder.Entity<TicketType>()
                .Property(x => x.Name)
                .HasMaxLength(60);

            modelBuilder.Entity<Showtime>()
                .Property(x => x.Status)
                .HasMaxLength(30);

            // NewsItem
            modelBuilder.Entity<NewsItem>()
                .Property(x => x.Title)
                .HasMaxLength(200);

            modelBuilder.Entity<NewsItem>()
                .Property(x => x.Summary)
                .HasMaxLength(300);

            modelBuilder.Entity<NewsItem>()
                .Property(x => x.ImageUrl)
                .HasMaxLength(500);

            // =========================
            // 👤 Usuario Admin (seed)
            // =========================
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Email = "admin@teatrouh.com",
                    Password = "Admin123*",
                    Role = Role.Admin
                }
            );

            // =========================
            // 🎭 Teatros (seed)
            // =========================
            modelBuilder.Entity<Theater>().HasData(
                new Theater
                {
                    TheaterId = 1,
                    Name = "Teatro Central",
                    Location = "San José",
                    Capacity = 500,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Theater
                {
                    TheaterId = 2,
                    Name = "TN",
                    Location = "SJ",
                    Capacity = 200,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Theater
                {
                    TheaterId = 3,
                    Name = "Teatro Nacional de Artes Escénicas y Representaciones Dramáticas de Gran Escala",
                    Location = "Centro Histórico de San José, Costa Rica",
                    Capacity = 900,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );

            // =========================
            // 🎬 Obras (seed)
            // =========================
            modelBuilder.Entity<Play>().HasData(
                new Play
                {
                    PlayId = 1,
                    Title = "Romeo y Julieta",
                    Description = "Obra clásica",
                    DurationMinutes = 120,
                    Rating = "A",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Play
                {
                    PlayId = 2,
                    Title = "La",
                    Description = "Título extremadamente corto para probar visual.",
                    DurationMinutes = 60,
                    Rating = "B",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Play
                {
                    PlayId = 3,
                    Title = "La increíble y absolutamente innecesariamente larga historia del teatro experimental contemporáneo",
                    Description = "Texto intencionalmente largo para estresar el diseño cuando el contenido supera el ancho normal del contenedor.",
                    DurationMinutes = 145,
                    Rating = "C",
                    ImageUrl = "https://via.placeholder.com/300x200",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );

            // =========================
            // 🎟️ Tipos de entrada (seed)
            // =========================
            modelBuilder.Entity<TicketType>().HasData(
                new TicketType { TicketTypeId = 1, Name = "General", PriceFactor = 1.0000m, IsActive = true },
                new TicketType { TicketTypeId = 2, Name = "VIP", PriceFactor = 1.5000m, IsActive = true }
            );

            // =========================
            // ⏰ Funciones (seed)
            // =========================
            modelBuilder.Entity<Showtime>().HasData(
                new Showtime
                {
                    ShowtimeId = 1,
                    PlayId = 1,
                    TheaterId = 1,
                    StartDateTime = new DateTime(2026, 2, 1, 19, 0, 0),
                    BasePrice = 5500m,
                    Status = "Scheduled",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Showtime
                {
                    ShowtimeId = 2,
                    PlayId = 1,
                    TheaterId = 1,
                    StartDateTime = new DateTime(2026, 2, 2, 19, 0, 0),
                    BasePrice = 5500m,
                    Status = "Scheduled",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Showtime
                {
                    ShowtimeId = 3,
                    PlayId = 2,
                    TheaterId = 2,
                    StartDateTime = new DateTime(2026, 2, 3, 15, 30, 0),
                    BasePrice = 3000m,
                    Status = "Scheduled",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Showtime
                {
                    ShowtimeId = 4,
                    PlayId = 3,
                    TheaterId = 3,
                    StartDateTime = new DateTime(2026, 2, 10, 20, 0, 0),
                    BasePrice = 12000m,
                    Status = "Scheduled",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                },
                new Showtime
                {
                    ShowtimeId = 5,
                    PlayId = 3,
                    TheaterId = 1,
                    StartDateTime = new DateTime(2026, 2, 11, 18, 45, 0),
                    BasePrice = 7500m,
                    Status = "Scheduled",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );

            // =========================
            // 📰 Noticias (seed)
            // =========================
            modelBuilder.Entity<NewsItem>().HasData(
                new NewsItem
                {
                    NewsItemId = 1,
                    Title = "Temporada 2026: nuevas funciones confirmadas",
                    Summary = "Se habilitan nuevas fechas para obras clásicas y producciones nacionales.",
                    Content = "El Teatro Nacional anuncia la apertura de nuevas fechas para la temporada 2026. Se incluyen funciones adicionales, horarios especiales y actividades para público familiar.",
                    ImageUrl = "https://via.placeholder.com/1200x600",
                    PublishedAt = new DateTime(2026, 1, 10, 10, 0, 0),
                    IsActive = true
                },
                new NewsItem
                {
                    NewsItemId = 2,
                    Title = "Mejoras en la experiencia de compra",
                    Summary = "Optimización del proceso de reserva y confirmación de entradas.",
                    Content = "Se actualizó el flujo de reserva para hacerlo más claro. Ahora el usuario puede revisar su carrito y confirmar con mayor facilidad.",
                    ImageUrl = "https://via.placeholder.com/1200x600",
                    PublishedAt = new DateTime(2026, 1, 15, 12, 0, 0),
                    IsActive = true
                },
                new NewsItem
                {
                    NewsItemId = 3,
                    Title = "Horario especial por mantenimiento",
                    Summary = "Algunas funciones podrían reprogramarse durante la semana de mantenimiento.",
                    Content = "Durante la semana de mantenimiento se estarán realizando trabajos internos. En caso de cambios en funciones, se notificará mediante esta sección.",
                    ImageUrl = "https://via.placeholder.com/1200x600",
                    PublishedAt = new DateTime(2026, 1, 20, 9, 30, 0),
                    IsActive = true
                }
            );
        }
    }
}
