using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UepaMed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarDuplicidadesIgnoradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DuplicidadesIgnoradas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RevisaoId = table.Column<int>(type: "integer", nullable: false),
                    ArtigoAId = table.Column<int>(type: "integer", nullable: false),
                    ArtigoBId = table.Column<int>(type: "integer", nullable: false),
                    DataDecisao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuplicidadesIgnoradas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuplicidadesIgnoradas");
        }
    }
}
