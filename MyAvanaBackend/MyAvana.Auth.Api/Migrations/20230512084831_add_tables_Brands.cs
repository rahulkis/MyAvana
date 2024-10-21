using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_tables_Brands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "BrandHairChallenges",
                columns: table => new
                {
                    BrandHairChallengeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairChallengeId = table.Column<int>(nullable: true),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandHairChallenges", x => x.BrandHairChallengeId);
                    table.ForeignKey(
                        name: "FK_BrandHairChallenges_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandHairChallenges_HairChallenges_HairChallengeId",
                        column: x => x.HairChallengeId,
                        principalTable: "HairChallenges",
                        principalColumn: "HairChallengeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandHairGoals",
                columns: table => new
                {
                    BrandHairGoalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairGoalId = table.Column<int>(nullable: true),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandHairGoals", x => x.BrandHairGoalId);
                    table.ForeignKey(
                        name: "FK_BrandHairGoals_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandHairGoals_HairGoals_HairGoalId",
                        column: x => x.HairGoalId,
                        principalTable: "HairGoals",
                        principalColumn: "HairGoalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrandHairTypes",
                columns: table => new
                {
                    BrandHairTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairTypeId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandHairTypes", x => x.BrandHairTypeId);
                    table.ForeignKey(
                        name: "FK_BrandHairTypes_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandHairTypes_HairTypes_HairTypeId",
                        column: x => x.HairTypeId,
                        principalTable: "HairTypes",
                        principalColumn: "HairTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandTags",
                columns: table => new
                {
                    BrandTagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TagsId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandTags", x => x.BrandTagId);
                    table.ForeignKey(
                        name: "FK_BrandTags_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandTags_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassificationBrands",
                columns: table => new
                {
                    ClassificationBrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandClassificationId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassificationBrands", x => x.ClassificationBrandId);
                    table.ForeignKey(
                        name: "FK_ClassificationBrands_BrandClassifications_BrandClassificationId",
                        column: x => x.BrandClassificationId,
                        principalTable: "BrandClassifications",
                        principalColumn: "BrandClassificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassificationBrands_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairChallenges_BrandId",
                table: "BrandHairChallenges",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairChallenges_HairChallengeId",
                table: "BrandHairChallenges",
                column: "HairChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairGoals_BrandId",
                table: "BrandHairGoals",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairGoals_HairGoalId",
                table: "BrandHairGoals",
                column: "HairGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairTypes_BrandId",
                table: "BrandHairTypes",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairTypes_HairTypeId",
                table: "BrandHairTypes",
                column: "HairTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandTags_BrandId",
                table: "BrandTags",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandTags_TagsId",
                table: "BrandTags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassificationBrands_BrandClassificationId",
                table: "ClassificationBrands",
                column: "BrandClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassificationBrands_BrandId",
                table: "ClassificationBrands",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandHairChallenges");

            migrationBuilder.DropTable(
                name: "BrandHairGoals");

            migrationBuilder.DropTable(
                name: "BrandHairTypes");

            migrationBuilder.DropTable(
                name: "BrandTags");

            migrationBuilder.DropTable(
                name: "ClassificationBrands");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
