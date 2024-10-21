using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class correctcolumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticleHairStyleId",
                table: "ArticleGuidances",
                newName: "ArticleGuidanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticleGuidanceId",
                table: "ArticleGuidances",
                newName: "ArticleHairStyleId");
        }
    }
}
