using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHome_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ApparaatStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Apparaten",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Apparaten");
        }
    }
}
