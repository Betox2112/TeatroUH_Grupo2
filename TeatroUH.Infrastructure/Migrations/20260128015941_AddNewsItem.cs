using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeatroUH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    NewsItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.NewsItemId);
                });

            migrationBuilder.InsertData(
                table: "NewsItems",
                columns: new[] { "NewsItemId", "Content", "ImageUrl", "IsActive", "PublishedAt", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "El Teatro Nacional anuncia la apertura de nuevas fechas para la temporada 2026. Se incluyen funciones adicionales, horarios especiales y actividades para público familiar.", "https://via.placeholder.com/1200x600", true, new DateTime(2026, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Se habilitan nuevas fechas para obras clásicas y producciones nacionales.", "Temporada 2026: nuevas funciones confirmadas" },
                    { 2, "Se actualizó el flujo de reserva para hacerlo más claro. Ahora el usuario puede revisar su carrito y confirmar con mayor facilidad.", "https://via.placeholder.com/1200x600", true, new DateTime(2026, 1, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), "Optimización del proceso de reserva y confirmación de entradas.", "Mejoras en la experiencia de compra" },
                    { 3, "Durante la semana de mantenimiento se estarán realizando trabajos internos. En caso de cambios en funciones, se notificará mediante esta sección.", "https://via.placeholder.com/1200x600", true, new DateTime(2026, 1, 20, 9, 30, 0, 0, DateTimeKind.Unspecified), "Algunas funciones podrían reprogramarse durante la semana de mantenimiento.", "Horario especial por mantenimiento" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItems");
        }
    }
}
