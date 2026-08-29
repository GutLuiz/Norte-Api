using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEstruturaVotacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Votacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisaoId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataFinalizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votacoes_Revisoes_RevisaoId",
                        column: x => x.RevisaoId,
                        principalTable: "Revisoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConflitosVotacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VotacaoId = table.Column<int>(type: "integer", nullable: false),
                    ArtigoId = table.Column<int>(type: "integer", nullable: false),
                    Motivo = table.Column<int>(type: "integer", nullable: false),
                    Resolvido = table.Column<bool>(type: "boolean", nullable: false),
                    DecisaoFinal = table.Column<int>(type: "integer", nullable: true),
                    AvaliadorId = table.Column<int>(type: "integer", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataResolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConflitosVotacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConflitosVotacao_Artigos_ArtigoId",
                        column: x => x.ArtigoId,
                        principalTable: "Artigos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConflitosVotacao_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VotacaoId = table.Column<int>(type: "integer", nullable: false),
                    ArtigoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Opcao = table.Column<int>(type: "integer", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votos_Artigos_ArtigoId",
                        column: x => x.ArtigoId,
                        principalTable: "Artigos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votos_Votacoes_VotacaoId",
                        column: x => x.VotacaoId,
                        principalTable: "Votacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConflitosVotacao_ArtigoId",
                table: "ConflitosVotacao",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConflitosVotacao_VotacaoId_ArtigoId",
                table: "ConflitosVotacao",
                columns: new[] { "VotacaoId", "ArtigoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votacoes_RevisaoId",
                table: "Votacoes",
                column: "RevisaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_ArtigoId",
                table: "Votos",
                column: "ArtigoId");

            migrationBuilder.CreateIndex(
                name: "IX_Votos_VotacaoId_ArtigoId_UsuarioId",
                table: "Votos",
                columns: new[] { "VotacaoId", "ArtigoId", "UsuarioId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConflitosVotacao");

            migrationBuilder.DropTable(
                name: "Votos");

            migrationBuilder.DropTable(
                name: "Votacoes");
        }
    }
}
