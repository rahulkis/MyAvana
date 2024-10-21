using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_HairAnalysisStatusHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StatusTracker",
                newName: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusTracker_CustomerId",
                table: "StatusTracker",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusTracker_AspNetUsers_CustomerId",
                table: "StatusTracker",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusTracker_AspNetUsers_CustomerId",
                table: "StatusTracker");

            migrationBuilder.DropIndex(
                name: "IX_StatusTracker_CustomerId",
                table: "StatusTracker");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "StatusTracker",
                newName: "UserId");
        }
    }
}
