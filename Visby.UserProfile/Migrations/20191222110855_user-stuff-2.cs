using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Karenia.Visby.UserProfile.Migrations
{
    public partial class userstuff2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "FavoriteList",
                table: "UserProfiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavoriteList",
                table: "UserProfiles");
        }
    }
}
