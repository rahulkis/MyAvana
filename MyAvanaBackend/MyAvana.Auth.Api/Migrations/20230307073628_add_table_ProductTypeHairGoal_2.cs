using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_ProductTypeHairGoal_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypeHairGoals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductTypeId = table.Column<int>(nullable: false),
                    HairGoalId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeHairGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairGoals_HairGoals_HairGoalId",
                        column: x => x.HairGoalId,
                        principalTable: "HairGoals",
                        principalColumn: "HairGoalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairGoals_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairGoals_HairGoalId",
                table: "ProductTypeHairGoals",
                column: "HairGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairGoals_ProductTypeId",
                table: "ProductTypeHairGoals",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypeHairGoals");
        }
    }
}
