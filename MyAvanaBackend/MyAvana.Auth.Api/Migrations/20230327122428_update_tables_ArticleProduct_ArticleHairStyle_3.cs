using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class update_tables_ArticleProduct_ArticleHairStyle_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleHairStyles_HairStyles_Id",
                table: "ArticleHairStyles");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleProducts_ProductEntities_Id",
                table: "ArticleProducts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ArticleProducts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleProducts_Id",
                table: "ArticleProducts",
                newName: "IX_ArticleProducts_ProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ArticleHairStyles",
                newName: "HairStyleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleHairStyles_Id",
                table: "ArticleHairStyles",
                newName: "IX_ArticleHairStyles_HairStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleHairStyles_HairStyles_HairStyleId",
                table: "ArticleHairStyles",
                column: "HairStyleId",
                principalTable: "HairStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleProducts_ProductEntities_ProductId",
                table: "ArticleProducts",
                column: "ProductId",
                principalTable: "ProductEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleHairStyles_HairStyles_HairStyleId",
                table: "ArticleHairStyles");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleProducts_ProductEntities_ProductId",
                table: "ArticleProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ArticleProducts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleProducts_ProductId",
                table: "ArticleProducts",
                newName: "IX_ArticleProducts_Id");

            migrationBuilder.RenameColumn(
                name: "HairStyleId",
                table: "ArticleHairStyles",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleHairStyles_HairStyleId",
                table: "ArticleHairStyles",
                newName: "IX_ArticleHairStyles_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleHairStyles_HairStyles_Id",
                table: "ArticleHairStyles",
                column: "Id",
                principalTable: "HairStyles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleProducts_ProductEntities_Id",
                table: "ArticleProducts",
                column: "Id",
                principalTable: "ProductEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
