using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace Karenia.Visby.Papers.Migrations
{
    public partial class searchvector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quote",
                table: "Papers");

            migrationBuilder.AddColumn<int>(
                name: "QuoteCount",
                table: "Papers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<string>>(
                name: "Quotes",
                table: "Papers",
                nullable: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Papers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    From = table.Column<int>(nullable: false),
                    By = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Papers_Authors",
                table: "Papers",
                column: "Authors")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_Keywords",
                table: "Papers",
                column: "Keywords")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Papers_SearchVector",
                table: "Papers",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_By",
                table: "Quotes",
                column: "By");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_From",
                table: "Quotes",
                column: "From");

            migrationBuilder.Sql(
                @"CREATE TRIGGER product_search_vector_update BEFORE INSERT OR UPDATE
                    ON ""Papers"" FOR EACH ROW EXECUTE PROCEDURE
                    tsvector_update_trigger(""SearchVector"", 'public.jiebaqry', ""Title"", ""Summary"");");

            // If you were adding a tsvector to an existing table, you should populate the column using an UPDATE
            migrationBuilder.Sql("UPDATE \"Papers\" SET \"Title\" = \"Title\";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Papers_Authors",
                table: "Papers");

            migrationBuilder.DropIndex(
                name: "IX_Papers_Keywords",
                table: "Papers");

            migrationBuilder.DropIndex(
                name: "IX_Papers_SearchVector",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "QuoteCount",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "Quotes",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Papers");

            migrationBuilder.AddColumn<int>(
                name: "Quote",
                table: "Papers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
