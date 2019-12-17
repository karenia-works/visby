using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Karenia.Visby.Papers.Migrations
{
    public partial class initializepaperdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Papers",
                columns: table => new
                {
                    PaperId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    Authors = table.Column<List<string>>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    PaperFromType = table.Column<int>(nullable: false),
                    PaperFrom = table.Column<string>(maxLength: 128, nullable: true),
                    Site = table.Column<string>(maxLength: 64, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Keywords = table.Column<List<string>>(nullable: true),
                    Quote = table.Column<int>(nullable: false),
                    LocalAuthorIds = table.Column<List<int>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papers", x => x.PaperId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Papers");
        }
    }
}
