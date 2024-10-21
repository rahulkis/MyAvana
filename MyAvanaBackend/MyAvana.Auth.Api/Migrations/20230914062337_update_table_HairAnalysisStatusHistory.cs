using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class update_table_HairAnalysisStatusHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairAnalysisStatusHistory_StatusTracker_NewStatusTrackerId",
                table: "HairAnalysisStatusHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_HairAnalysisStatusHistory_StatusTracker_OldStatusTrackerId",
                table: "HairAnalysisStatusHistory");

            migrationBuilder.RenameColumn(
                name: "OldStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                newName: "OldHairAnalysisStatusId");

            migrationBuilder.RenameColumn(
                name: "NewStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                newName: "NewHairAnalysisStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_HairAnalysisStatusHistory_OldStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                newName: "IX_HairAnalysisStatusHistory_OldHairAnalysisStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_HairAnalysisStatusHistory_NewStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                newName: "IX_HairAnalysisStatusHistory_NewHairAnalysisStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_HairAnalysisStatusHistory_HairAnalysisStatus_NewHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                column: "NewHairAnalysisStatusId",
                principalTable: "HairAnalysisStatus",
                principalColumn: "HairAnalysisStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairAnalysisStatusHistory_HairAnalysisStatus_OldHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                column: "OldHairAnalysisStatusId",
                principalTable: "HairAnalysisStatus",
                principalColumn: "HairAnalysisStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairAnalysisStatusHistory_HairAnalysisStatus_NewHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_HairAnalysisStatusHistory_HairAnalysisStatus_OldHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory");

            migrationBuilder.RenameColumn(
                name: "OldHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                newName: "OldStatusTrackerId");

            migrationBuilder.RenameColumn(
                name: "NewHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                newName: "NewStatusTrackerId");

            migrationBuilder.RenameIndex(
                name: "IX_HairAnalysisStatusHistory_OldHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                newName: "IX_HairAnalysisStatusHistory_OldStatusTrackerId");

            migrationBuilder.RenameIndex(
                name: "IX_HairAnalysisStatusHistory_NewHairAnalysisStatusId",
                table: "HairAnalysisStatusHistory",
                newName: "IX_HairAnalysisStatusHistory_NewStatusTrackerId");

            migrationBuilder.AddForeignKey(
                name: "FK_HairAnalysisStatusHistory_StatusTracker_NewStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                column: "NewStatusTrackerId",
                principalTable: "StatusTracker",
                principalColumn: "StatusTrackerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairAnalysisStatusHistory_StatusTracker_OldStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                column: "OldStatusTrackerId",
                principalTable: "StatusTracker",
                principalColumn: "StatusTrackerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
