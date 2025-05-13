using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class Game_009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTag",
                table: "RoomTag");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoomTag",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTag",
                table: "RoomTag",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTag_RoomId",
                table: "RoomTag",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTag",
                table: "RoomTag");

            migrationBuilder.DropIndex(
                name: "IX_RoomTag_RoomId",
                table: "RoomTag");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoomTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTag",
                table: "RoomTag",
                column: "RoomId");
        }
    }
}
