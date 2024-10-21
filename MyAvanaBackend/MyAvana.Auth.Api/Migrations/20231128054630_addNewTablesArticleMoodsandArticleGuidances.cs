using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addNewTablesArticleMoodsandArticleGuidances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guidances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Guidance = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guidances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Mood = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleGuidances",
                columns: table => new
                {
                    ArticleHairStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BlogArticleId = table.Column<int>(nullable: true),
                    GuidanceId = table.Column<int>(nullable: true),
                    GuidancesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleGuidances", x => x.ArticleHairStyleId);
                    table.ForeignKey(
                        name: "FK_ArticleGuidances_BlogArticles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "BlogArticles",
                        principalColumn: "BlogArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleGuidances_Guidances_GuidancesId",
                        column: x => x.GuidancesId,
                        principalTable: "Guidances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleMoods",
                columns: table => new
                {
                    ArticleMoodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    BlogArticleId = table.Column<int>(nullable: true),
                    MoodId = table.Column<int>(nullable: true),
                    MoodsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleMoods", x => x.ArticleMoodId);
                    table.ForeignKey(
                        name: "FK_ArticleMoods_BlogArticles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "BlogArticles",
                        principalColumn: "BlogArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleMoods_Moods_MoodsId",
                        column: x => x.MoodsId,
                        principalTable: "Moods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleGuidances_BlogArticleId",
                table: "ArticleGuidances",
                column: "BlogArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleGuidances_GuidancesId",
                table: "ArticleGuidances",
                column: "GuidancesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMoods_BlogArticleId",
                table: "ArticleMoods",
                column: "BlogArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleMoods_MoodsId",
                table: "ArticleMoods",
                column: "MoodsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleGuidances");

            migrationBuilder.DropTable(
                name: "ArticleMoods");

            migrationBuilder.DropTable(
                name: "Guidances");

            migrationBuilder.DropTable(
                name: "Moods");
        }
    }
}
