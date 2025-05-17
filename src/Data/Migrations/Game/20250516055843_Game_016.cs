using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class Game_016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBannedWord_Rooms_RoomId",
                table: "RoomBannedWord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomBannedWord",
                table: "RoomBannedWord");

            migrationBuilder.RenameTable(
                name: "RoomBannedWord",
                newName: "RoomBannedWords");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomBannedWords",
                table: "RoomBannedWords",
                columns: new[] { "RoomId", "BannedWord" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBannedWords_Rooms_RoomId",
                table: "RoomBannedWords",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBannedWords_Rooms_RoomId",
                table: "RoomBannedWords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomBannedWords",
                table: "RoomBannedWords");

            migrationBuilder.RenameTable(
                name: "RoomBannedWords",
                newName: "RoomBannedWord");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomBannedWord",
                table: "RoomBannedWord",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBannedWord_Rooms_RoomId",
                table: "RoomBannedWord",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
