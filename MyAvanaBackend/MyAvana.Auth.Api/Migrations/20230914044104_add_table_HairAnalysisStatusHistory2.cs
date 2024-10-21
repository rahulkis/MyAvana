using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_HairAnalysisStatusHistory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HairAnalysisStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OldStatusTrackerId = table.Column<int>(nullable: true),
                    NewStatusTrackerId = table.Column<int>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UpdatedByUserId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairAnalysisStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairAnalysisStatusHistory_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HairAnalysisStatusHistory_StatusTracker_NewStatusTrackerId",
                        column: x => x.NewStatusTrackerId,
                        principalTable: "StatusTracker",
                        principalColumn: "StatusTrackerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HairAnalysisStatusHistory_StatusTracker_OldStatusTrackerId",
                        column: x => x.OldStatusTrackerId,
                        principalTable: "StatusTracker",
                        principalColumn: "StatusTrackerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HairAnalysisStatusHistory_WebLogins_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "WebLogins",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HairAnalysisStatusHistory_CustomerId",
                table: "HairAnalysisStatusHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_HairAnalysisStatusHistory_NewStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                column: "NewStatusTrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_HairAnalysisStatusHistory_OldStatusTrackerId",
                table: "HairAnalysisStatusHistory",
                column: "OldStatusTrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_HairAnalysisStatusHistory_UpdatedByUserId",
                table: "HairAnalysisStatusHistory",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HairAnalysisStatusHistory");
        }
    }
}
