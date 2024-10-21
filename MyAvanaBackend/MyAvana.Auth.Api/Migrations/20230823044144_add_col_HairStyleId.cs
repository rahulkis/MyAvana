using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_HairStyleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HairStyleId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HairStylesId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_HairStylesId",
                table: "ProductCommons",
                column: "HairStylesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_HairStyles_HairStylesId",
                table: "ProductCommons",
                column: "HairStylesId",
                principalTable: "HairStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_HairStyles_HairStylesId",
                table: "ProductCommons");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_HairStylesId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "HairStyleId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "HairStylesId",
                table: "ProductCommons");
        }
    }
}
