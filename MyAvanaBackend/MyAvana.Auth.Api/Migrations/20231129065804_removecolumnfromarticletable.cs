using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class removecolumnfromarticletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guidance",
                table: "BlogArticles");

            migrationBuilder.DropColumn(
                name: "Mood",
                table: "BlogArticles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guidance",
                table: "BlogArticles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "BlogArticles",
                nullable: true);
        }
    }
}
