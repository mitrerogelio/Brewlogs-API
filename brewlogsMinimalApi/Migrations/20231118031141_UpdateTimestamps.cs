using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace brewlogsMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CoffeeName",
                value: "Mugshots Chiapas");

            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CoffeeName", "Roast" },
                values: new object[] { "Caribou Light Roast", "Light" });

            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CoffeeName",
                value: "Five Watt Guatemala");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CoffeeName",
                value: "Coffee 1");

            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CoffeeName", "Roast" },
                values: new object[] { "Coffee 2", "Dark" });

            migrationBuilder.UpdateData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CoffeeName",
                value: "Coffee 3");
        }
    }
}
