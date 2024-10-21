using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class update_tables_ArticleProduct_ArticleHairStyle_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ArticleProducts");

            migrationBuilder.DropColumn(
                name: "HairStyleId",
                table: "ArticleHairStyles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ArticleProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HairStyleId",
                table: "ArticleHairStyles",
                nullable: true);
        }
    }
}
