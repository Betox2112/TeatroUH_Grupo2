using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeatroUH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStringLengths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TicketTypes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Theaters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Theaters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Showtimes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Plays",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Plays",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Plays",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Plays",
                columns: new[] { "PlayId", "CreatedAt", "Description", "DurationMinutes", "ImageUrl", "IsActive", "Rating", "Title" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Título extremadamente corto para probar visual.", 60, "https://via.placeholder.com/300x200", true, "B", "La" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Texto intencionalmente largo para estresar el diseño cuando el contenido supera el ancho normal del contenedor.", 145, "https://via.placeholder.com/300x200", true, "C", "La increíble y absolutamente innecesariamente larga historia del teatro experimental contemporáneo" }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "ShowtimeId", "BasePrice", "CreatedAt", "PlayId", "StartDateTime", "Status", "TheaterId" },
                values: new object[] { 2, 5500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 2, 19, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled", 1 });

            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "TheaterId", "Capacity", "CreatedAt", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { 2, 200, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "SJ", "TN" },
                    { 3, 900, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Centro Histórico de San José, Costa Rica", "Teatro Nacional de Artes Escénicas y Representaciones Dramáticas de Gran Escala" }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "ShowtimeId", "BasePrice", "CreatedAt", "PlayId", "StartDateTime", "Status", "TheaterId" },
                values: new object[,]
                {
                    { 3, 3000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 2, 3, 15, 30, 0, 0, DateTimeKind.Unspecified), "Scheduled", 2 },
                    { 4, 12000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 2, 10, 20, 0, 0, 0, DateTimeKind.Unspecified), "Scheduled", 3 },
                    { 5, 7500m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 2, 11, 18, 45, 0, 0, DateTimeKind.Unspecified), "Scheduled", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Plays",
                keyColumn: "PlayId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plays",
                keyColumn: "PlayId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "TheaterId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "TheaterId",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TicketTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Theaters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Theaters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Showtimes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Plays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Plays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Plays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(600)",
                oldMaxLength: 600,
                oldNullable: true);
        }
    }
}
