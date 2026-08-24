using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaConviteRevisao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConvitesRevisao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisaoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioConvidadoId = table.Column<int>(type: "integer", nullable: false),
                    ConvidadoPorUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RespondidoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvitesRevisao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvitesRevisao_Revisoes_RevisaoId",
                        column: x => x.RevisaoId,
                        principalTable: "Revisoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConvitesRevisao_Usuarios_ConvidadoPorUsuarioId",
                        column: x => x.ConvidadoPorUsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConvitesRevisao_Usuarios_UsuarioConvidadoId",
                        column: x => x.UsuarioConvidadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvitesRevisao_ConvidadoPorUsuarioId",
                table: "ConvitesRevisao",
                column: "ConvidadoPorUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvitesRevisao_RevisaoId_UsuarioConvidadoId_Status",
                table: "ConvitesRevisao",
                columns: new[] { "RevisaoId", "UsuarioConvidadoId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ConvitesRevisao_UsuarioConvidadoId",
                table: "ConvitesRevisao",
                column: "UsuarioConvidadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvitesRevisao");
        }
    }
}
