using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_BrandClassificationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandClassificationId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_BrandClassificationId",
                table: "ProductCommons",
                column: "BrandClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_BrandClassifications_BrandClassificationId",
                table: "ProductCommons",
                column: "BrandClassificationId",
                principalTable: "BrandClassifications",
                principalColumn: "BrandClassificationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_BrandClassifications_BrandClassificationId",
                table: "ProductCommons");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_BrandClassificationId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "BrandClassificationId",
                table: "ProductCommons");
        }
    }
}
