using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_HideInSearch_BrandsAndProductEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideInSearch",
                table: "ProductEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HideInSearch",
                table: "Brands",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HideInSearch",
                table: "ProductEntities");

            migrationBuilder.DropColumn(
                name: "HideInSearch",
                table: "Brands");
        }
    }
}
