using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_ProductTypeHairChallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypeHairChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductTypeId = table.Column<int>(nullable: false),
                    HairChallengeId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeHairChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairChallenges_HairChallenges_HairChallengeId",
                        column: x => x.HairChallengeId,
                        principalTable: "HairChallenges",
                        principalColumn: "HairChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairChallenges_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairChallenges_HairChallengeId",
                table: "ProductTypeHairChallenges",
                column: "HairChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairChallenges_ProductTypeId",
                table: "ProductTypeHairChallenges",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypeHairChallenges");
        }
    }
}
