using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace brewlogsMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brewlogs",
                columns: new[] { "Id", "BrewRatio", "BrewerUsed", "CoffeeName", "Dose", "Grind", "Roast" },
                values: new object[,]
                {
                    { 1, 15, "AeroPress", "Coffee 1", 20, "Medium", "Medium" },
                    { 2, 16, "Pour-Over", "Coffee 2", 18, "Fine", "Dark" },
                    { 3, 16, "Pour-Over", "Coffee 3", 18, "Fine", "Light" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brewlogs",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
