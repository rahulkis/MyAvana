using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_CustomerPreferenceId_ProductCommon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerPreferenceId",
                table: "ProductCommons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommons_CustomerPreferenceId",
                table: "ProductCommons",
                column: "CustomerPreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCommons_CustomerPreference_CustomerPreferenceId",
                table: "ProductCommons",
                column: "CustomerPreferenceId",
                principalTable: "CustomerPreference",
                principalColumn: "CustomerPreferenceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCommons_CustomerPreference_CustomerPreferenceId",
                table: "ProductCommons");

            migrationBuilder.DropIndex(
                name: "IX_ProductCommons_CustomerPreferenceId",
                table: "ProductCommons");

            migrationBuilder.DropColumn(
                name: "CustomerPreferenceId",
                table: "ProductCommons");
        }
    }
}
