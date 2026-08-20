using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UepaMed.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioIdToRevisao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Revisoes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Revisoes_UsuarioId",
                table: "Revisoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revisoes_Usuarios_UsuarioId",
                table: "Revisoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revisoes_Usuarios_UsuarioId",
                table: "Revisoes");

            migrationBuilder.DropIndex(
                name: "IX_Revisoes_UsuarioId",
                table: "Revisoes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Revisoes");
        }
    }
}
