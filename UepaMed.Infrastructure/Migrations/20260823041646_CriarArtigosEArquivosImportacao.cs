using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarArtigosEArquivosImportacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArquivosImportacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisaoId = table.Column<int>(type: "integer", nullable: false),
                    NomeArquivo = table.Column<string>(type: "text", nullable: false),
                    TipoArquivo = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeArtigos = table.Column<int>(type: "integer", nullable: false),
                    DataImportacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosImportacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosImportacao_Revisoes_RevisaoId",
                        column: x => x.RevisaoId,
                        principalTable: "Revisoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artigos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisaoId = table.Column<int>(type: "integer", nullable: false),
                    ArquivoImportacaoId = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Resumo = table.Column<string>(type: "text", nullable: true),
                    Autores = table.Column<string>(type: "text", nullable: true),
                    Revista = table.Column<string>(type: "text", nullable: true),
                    AnoPublicacao = table.Column<int>(type: "integer", nullable: true),
                    DOI = table.Column<string>(type: "text", nullable: true),
                    PMID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artigos_ArquivosImportacao_ArquivoImportacaoId",
                        column: x => x.ArquivoImportacaoId,
                        principalTable: "ArquivosImportacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Artigos_Revisoes_RevisaoId",
                        column: x => x.RevisaoId,
                        principalTable: "Revisoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosImportacao_RevisaoId",
                table: "ArquivosImportacao",
                column: "RevisaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_ArquivoImportacaoId",
                table: "Artigos",
                column: "ArquivoImportacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_RevisaoId",
                table: "Artigos",
                column: "RevisaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artigos");

            migrationBuilder.DropTable(
                name: "ArquivosImportacao");
        }
    }
}
