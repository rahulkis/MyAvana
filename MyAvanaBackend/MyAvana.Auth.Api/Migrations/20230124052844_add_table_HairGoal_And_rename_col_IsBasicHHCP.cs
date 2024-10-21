using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_HairGoal_And_rename_col_IsBasicHHCP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCreatedByDigitalAssessment",
                table: "HairProfiles",
                newName: "IsBasicHHCP");

            migrationBuilder.AddColumn<int>(
                name: "HairGoalId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HairGoals",
                columns: table => new
                {
                    HairGoalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairGoals", x => x.HairGoalId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_HairGoalId",
                table: "ProductCommons",
                column: "HairGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_HairGoals_HairGoalId",
                table: "ProductCommons",
                column: "HairGoalId",
                principalTable: "HairGoals",
                principalColumn: "HairGoalId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_HairGoals_HairGoalId",
                table: "ProductCommons");

            migrationBuilder.DropTable(
                name: "HairGoals");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_HairGoalId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "HairGoalId",
                table: "ProductCommons");

            migrationBuilder.RenameColumn(
                name: "IsBasicHHCP",
                table: "HairProfiles",
                newName: "IsCreatedByDigitalAssessment");
        }
    }
}
