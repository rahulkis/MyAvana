using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_SalonLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalonLogo",
                table: "Salons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalonLogo",
                table: "Salons");
        }
    }
}
