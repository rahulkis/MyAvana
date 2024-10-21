using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_fkCol_ProductTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_ProductTypeId",
                table: "ProductCommons",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_ProductTypes_ProductTypeId",
                table: "ProductCommons",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_ProductTypes_ProductTypeId",
                table: "ProductCommons");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_ProductTypeId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "ProductCommons");
        }
    }
}
