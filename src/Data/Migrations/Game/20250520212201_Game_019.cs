using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class Game_019 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Rooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Rooms");
        }
    }
}
