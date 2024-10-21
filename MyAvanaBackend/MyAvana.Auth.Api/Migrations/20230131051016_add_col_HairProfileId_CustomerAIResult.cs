using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_HairProfileId_CustomerAIResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HairProfileId",
                table: "CustomerAIResults",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAIResults_HairProfileId",
                table: "CustomerAIResults",
                column: "HairProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAIResults_HairProfiles_HairProfileId",
                table: "CustomerAIResults",
                column: "HairProfileId",
                principalTable: "HairProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAIResults_HairProfiles_HairProfileId",
                table: "CustomerAIResults");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAIResults_HairProfileId",
                table: "CustomerAIResults");

            migrationBuilder.DropColumn(
                name: "HairProfileId",
                table: "CustomerAIResults");
        }
    }
}
