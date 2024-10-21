using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_tables_ArticleProduct_ArticleHairStyle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleHairStyles",
                columns: table => new
                {
                    ArticleHairStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<int>(nullable: false),
                    HairStyleId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BlogArticleId = table.Column<int>(nullable: true),
                    HairStylesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleHairStyles", x => x.ArticleHairStyleId);
                    table.ForeignKey(
                        name: "FK_ArticleHairStyles_BlogArticles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "BlogArticles",
                        principalColumn: "BlogArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleHairStyles_HairStyles_HairStylesId",
                        column: x => x.HairStylesId,
                        principalTable: "HairStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleProducts",
                columns: table => new
                {
                    ArticleProductsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BlogArticleId = table.Column<int>(nullable: true),
                    ProductEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleProducts", x => x.ArticleProductsId);
                    table.ForeignKey(
                        name: "FK_ArticleProducts_BlogArticles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "BlogArticles",
                        principalColumn: "BlogArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleProducts_ProductEntities_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "ProductEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHairStyles_BlogArticleId",
                table: "ArticleHairStyles",
                column: "BlogArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleHairStyles_HairStylesId",
                table: "ArticleHairStyles",
                column: "HairStylesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleProducts_BlogArticleId",
                table: "ArticleProducts",
                column: "BlogArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleProducts_ProductEntityId",
                table: "ArticleProducts",
                column: "ProductEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleHairStyles");

            migrationBuilder.DropTable(
                name: "ArticleProducts");
        }
    }
}
