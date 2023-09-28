using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TipoPersona_TipoPersonaId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TipoPersona");

            migrationBuilder.DropIndex(
                name: "IX_Users_TipoPersonaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TipoPersonaId",
                table: "Users");
        }
    }
}