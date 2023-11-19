using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace brewlogsMinimalApi.Migrations;

/// <inheritdoc />
public partial class GuidAndAuthor : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
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

        migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Brewlogs",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Brewlogs",
                type: "longtext",
                nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Author",
            table: "Brewlogs");

        migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Brewlogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.InsertData(
            table: "Brewlogs",
            columns: new[] { "Id", "BrewRatio", "BrewerUsed", "CoffeeName", "Dose", "Grind", "Roast" },
            values: new object[,]
            {
                { 1, 15, "AeroPress", "Mugshots Chiapas", 20, "Medium", "Medium" },
                { 2, 16, "Pour-Over", "Caribou Light Roast", 18, "Fine", "Light" },
                { 3, 16, "Pour-Over", "Five Watt Guatemala", 18, "Fine", "Light" }
            });
    }
}