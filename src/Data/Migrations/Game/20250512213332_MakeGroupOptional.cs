using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iso.Data.Migrations.Game
{
    /// <inheritdoc />
    public partial class MakeGroupOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Rooms",
                nullable: true,
                defaultValueSql: "NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
