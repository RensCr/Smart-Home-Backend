using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome_Backend.Migrations
{
    /// <inheritdoc />
    public partial class GebruikerWoonplaatst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WoonPlaats",
                table: "Gebruikers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WoonPlaats",
                table: "Gebruikers");
        }
    }
}
