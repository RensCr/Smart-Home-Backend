using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome_Backend.Migrations
{
    /// <inheritdoc />
    public partial class HuizenPlattegrond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApparatenJson",
                table: "Huizen",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KamersJson",
                table: "Huizen",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApparatenJson",
                table: "Huizen");

            migrationBuilder.DropColumn(
                name: "KamersJson",
                table: "Huizen");
        }
    }
}
