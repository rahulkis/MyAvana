using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_BrandId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "ProductEntities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntities_BrandId",
                table: "ProductEntities",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntities_Brands_BrandId",
                table: "ProductEntities",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntities_Brands_BrandId",
                table: "ProductEntities");

            migrationBuilder.DropIndex(
                name: "IX_ProductEntities_BrandId",
                table: "ProductEntities");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "ProductEntities");
        }
    }
}
