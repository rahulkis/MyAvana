using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_HairAnalysisStatus_And_StatusTracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HairAnalysisStatus",
                columns: table => new
                {
                    HairAnalysisStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairAnalysisStatus", x => x.HairAnalysisStatusId);
                });

            migrationBuilder.CreateTable(
                name: "StatusTracker",
                columns: table => new
                {
                    StatusTrackerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    KitSerialNumber = table.Column<string>(nullable: true),
                    HairAnalysisStatusId = table.Column<int>(nullable: false),
                    WelcomeEmailSent = table.Column<bool>(nullable: false),
                    WelcomeEmailSentOn = table.Column<DateTime>(nullable: true),
                    EmailCommunicationLastSentOn = table.Column<DateTime>(nullable: true),
                    EmailCommunicationCount = table.Column<int>(nullable: false),
                    InAppNotificationLastSentOn = table.Column<DateTime>(nullable: true),
                    InAppNotificationCount = table.Column<int>(nullable: false),
                    ExtensionRequested = table.Column<bool>(nullable: false),
                    ExtensionDate = table.Column<DateTime>(nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTracker", x => x.StatusTrackerId);
                    table.ForeignKey(
                        name: "FK_StatusTracker_HairAnalysisStatus_HairAnalysisStatusId",
                        column: x => x.HairAnalysisStatusId,
                        principalTable: "HairAnalysisStatus",
                        principalColumn: "HairAnalysisStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusTracker_HairAnalysisStatusId",
                table: "StatusTracker",
                column: "HairAnalysisStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusTracker");

            migrationBuilder.DropTable(
                name: "HairAnalysisStatus");
        }
    }
}
