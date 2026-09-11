using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarParticipantesDaVotacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotacaoParticipantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VotacaoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Papel = table.Column<int>(type: "integer", nullable: false),
                    EhVotanteObrigatorio = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotacaoParticipantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotacaoParticipantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotacaoParticipantes_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotacaoParticipantes_UsuarioId",
                table: "VotacaoParticipantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_VotacaoParticipantes_VotacaoId_UsuarioId",
                table: "VotacaoParticipantes",
                columns: new[] { "VotacaoId", "UsuarioId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotacaoParticipantes");
        }
    }
}
