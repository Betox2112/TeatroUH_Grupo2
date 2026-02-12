using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeatroUH.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShowtimeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Showtimes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatsSold",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 1,
                columns: new[] { "Capacity", "IsActive", "SeatsSold" },
                values: new object[] { 1500, true, 0 });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 2,
                columns: new[] { "Capacity", "IsActive", "SeatsSold" },
                values: new object[] { 1500, true, 0 });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 3,
                columns: new[] { "Capacity", "IsActive", "SeatsSold" },
                values: new object[] { 1500, true, 0 });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 4,
                columns: new[] { "Capacity", "IsActive", "SeatsSold" },
                values: new object[] { 1500, true, 0 });

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "ShowtimeId",
                keyValue: 5,
                columns: new[] { "Capacity", "IsActive", "SeatsSold" },
                values: new object[] { 1500, true, 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Nombre", "Password", "Role" },
                values: new object[] { 1, "admin@teatrouh.com", "Administrador", "Admin123*", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "SeatsSold",
                table: "Showtimes");
        }
    }
}
