using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_StylistNotesHHCP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StylistNotesHHCPs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Notes = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    HairProfileId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StylistNotesHHCPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StylistNotesHHCPs_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StylistNotesHHCPs_WebLogins_UserId",
                        column: x => x.UserId,
                        principalTable: "WebLogins",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StylistNotesHHCPs_HairProfileId",
                table: "StylistNotesHHCPs",
                column: "HairProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_StylistNotesHHCPs_UserId",
                table: "StylistNotesHHCPs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StylistNotesHHCPs");
        }
    }
}
