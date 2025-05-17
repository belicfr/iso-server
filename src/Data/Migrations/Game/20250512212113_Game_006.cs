using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class Game_006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
