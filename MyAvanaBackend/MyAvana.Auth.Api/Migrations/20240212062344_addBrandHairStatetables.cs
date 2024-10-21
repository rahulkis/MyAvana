using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addBrandHairStatetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.CreateTable(
                name: "BrandRecommendationStatus",
                columns: table => new
                {
                    BrandRecommendationStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandRecommendationStatus", x => x.BrandRecommendationStatusId);
                });

            migrationBuilder.CreateTable(
                name: "HairState",
                columns: table => new
                {
                    HairStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairState", x => x.HairStateId);
                });

            migrationBuilder.CreateTable(
                name: "MolecularWeight",
                columns: table => new
                {
                    MolecularWeightId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MolecularWeight", x => x.MolecularWeightId);
                });

            migrationBuilder.CreateTable(
                name: "BrandsBrandRecommendationStatus",
                columns: table => new
                {
                    BrandsBrandRecommendationStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrandRecommendationStatusId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandsBrandRecommendationStatus", x => x.BrandsBrandRecommendationStatusId);
                    table.ForeignKey(
                        name: "FK_BrandsBrandRecommendationStatus_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandsBrandRecommendationStatus_BrandRecommendationStatus_BrandRecommendationStatusId",
                        column: x => x.BrandRecommendationStatusId,
                        principalTable: "BrandRecommendationStatus",
                        principalColumn: "BrandRecommendationStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandHairState",
                columns: table => new
                {
                    BrandHairStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairStateId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandHairState", x => x.BrandHairStateId);
                    table.ForeignKey(
                        name: "FK_BrandHairState_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandHairState_HairState_HairStateId",
                        column: x => x.HairStateId,
                        principalTable: "HairState",
                        principalColumn: "HairStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandMolecularWeight",
                columns: table => new
                {
                    BrandMolecularWeightId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MolecularWeightId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandMolecularWeight", x => x.BrandMolecularWeightId);
                    table.ForeignKey(
                        name: "FK_BrandMolecularWeight_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandMolecularWeight_MolecularWeight_MolecularWeightId",
                        column: x => x.MolecularWeightId,
                        principalTable: "MolecularWeight",
                        principalColumn: "MolecularWeightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairState_BrandId",
                table: "BrandHairState",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandHairState_HairStateId",
                table: "BrandHairState",
                column: "HairStateId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandMolecularWeight_BrandId",
                table: "BrandMolecularWeight",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandMolecularWeight_MolecularWeightId",
                table: "BrandMolecularWeight",
                column: "MolecularWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandsBrandRecommendationStatus_BrandId",
                table: "BrandsBrandRecommendationStatus",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandsBrandRecommendationStatus_BrandRecommendationStatusId",
                table: "BrandsBrandRecommendationStatus",
                column: "BrandRecommendationStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandHairState");

            migrationBuilder.DropTable(
                name: "BrandMolecularWeight");

            migrationBuilder.DropTable(
                name: "BrandsBrandRecommendationStatus");

            migrationBuilder.DropTable(
                name: "HairState");

            migrationBuilder.DropTable(
                name: "MolecularWeight");

            migrationBuilder.DropTable(
                name: "BrandRecommendationStatus");

           
        }
    }
}
