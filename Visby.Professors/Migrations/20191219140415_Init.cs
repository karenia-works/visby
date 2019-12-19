using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Karenia.Visby.Professors.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    ProfessorId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Contract = table.Column<string>(maxLength: 128, nullable: true),
                    Institution = table.Column<string>(maxLength: 128, nullable: true),
                    ReachFields = table.Column<List<string>>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.ProfessorId);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorApplies",
                columns: table => new
                {
                    ProfessorApplyId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplyDate = table.Column<DateTime>(nullable: false),
                    ApplyState = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Contract = table.Column<string>(maxLength: 128, nullable: true),
                    Institution = table.Column<string>(maxLength: 128, nullable: true),
                    CertificateDocument = table.Column<string>(maxLength: 2048, nullable: true),
                    ProfessorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorApplies", x => x.ProfessorApplyId);
                    table.ForeignKey(
                        name: "FK_ProfessorApplies_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "ProfessorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorApplies_ProfessorId",
                table: "ProfessorApplies",
                column: "ProfessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessorApplies");

            migrationBuilder.DropTable(
                name: "Professors");
        }
    }
}
