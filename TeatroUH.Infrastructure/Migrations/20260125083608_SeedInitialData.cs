using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeatroUH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceFactor",
                table: "TicketTypes",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Plays",
                columns: new[] { "PlayId", "CreatedAt", "Description", "DurationMinutes", "ImageUrl", "IsActive", "Rating", "Title" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Obra clásica", 120, "https://via.placeholder.com/300x200", true, "A", "Romeo y Julieta" });

            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "TheaterId", "Capacity", "CreatedAt", "IsActive", "Location", "Name" },
                values: new object[] { 1, 500, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "San José", "Teatro Central" });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "TicketTypeId", "IsActive", "Name", "PriceFactor" },
                values: new object[,]
                {
                    { 1, true, "General", 1.0m },
                    { 2, true, "VIP", 1.5m }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "ShowtimeId", "BasePrice", "CreatedAt", "PlayId", "StartDateTime", "Status", "TheaterId" },
                values: new object[] { 1, 5500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plays",
                keyColumn: "PlayId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "TheaterId",
                keyValue: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceFactor",
                table: "TicketTypes",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);
        }
    }
}
