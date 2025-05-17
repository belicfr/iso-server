using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class RefactorAnnotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_GroupId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Groups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_RoomId",
                table: "Groups",
                column: "RoomId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Rooms_RoomId",
                table: "Groups",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Rooms_RoomId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_RoomId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_GroupId",
                table: "Rooms",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Groups_GroupId",
                table: "Rooms",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
