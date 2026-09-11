using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarArtigosDaVotacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotacaoArtigos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VotacaoId = table.Column<int>(type: "integer", nullable: false),
                    ArtigoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotacaoArtigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotacaoArtigos_Artigos_ArtigoId",
                        column: x => x.ArtigoId,
                        principalTable: "Artigos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VotacaoArtigos_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VotacaoArtigos_ArtigoId",
                table: "VotacaoArtigos",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_VotacaoArtigos_VotacaoId_ArtigoId",
                table: "VotacaoArtigos",
                columns: new[] { "VotacaoId", "ArtigoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotacaoArtigos");
        }
    }
}
