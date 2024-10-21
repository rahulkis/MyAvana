using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addedtablesforstylerecipes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecommendedProductsStyleRecipe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    HairProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedProductsStyleRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedProductsStyleRecipe_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedStyleRecipeVideos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    MediaLinkEntityId = table.Column<int>(nullable: false),
                    ThumbNail = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    HairProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedStyleRecipeVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedStyleRecipeVideos_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedProductsStyleRecipe_HairProfileId",
                table: "RecommendedProductsStyleRecipe",
                column: "HairProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedStyleRecipeVideos_HairProfileId",
                table: "RecommendedStyleRecipeVideos",
                column: "HairProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendedProductsStyleRecipe");

            migrationBuilder.DropTable(
                name: "RecommendedStyleRecipeVideos");
        }
    }
}
