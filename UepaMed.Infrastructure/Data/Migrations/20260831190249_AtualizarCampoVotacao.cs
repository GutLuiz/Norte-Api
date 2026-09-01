using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UepaMed.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarCampoVotacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votacoes_Revisoes_RevisaoId",
                table: "Votacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Votacoes_Revisoes_RevisaoId",
                table: "Votacoes",
                column: "RevisaoId",
                principalTable: "Revisoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votacoes_Revisoes_RevisaoId",
                table: "Votacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Votacoes_Revisoes_RevisaoId",
                table: "Votacoes",
                column: "RevisaoId",
                principalTable: "Revisoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
