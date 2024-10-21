using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addTablesToSaveVideoMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HairChallengeVideoMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MediaLinkEntitiesId = table.Column<int>(nullable: false),
                    MediaLinkEntityId = table.Column<int>(nullable: true),
                    HairChallengeId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairChallengeVideoMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairChallengeVideoMapping_HairChallenges_HairChallengeId",
                        column: x => x.HairChallengeId,
                        principalTable: "HairChallenges",
                        principalColumn: "HairChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HairChallengeVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                        column: x => x.MediaLinkEntityId,
                        principalTable: "MediaLinkEntities",
                        principalColumn: "MediaLinkEntityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HairGoalVideoMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MediaLinkEntitiesId = table.Column<int>(nullable: false),
                    MediaLinkEntityId = table.Column<int>(nullable: true),
                    HairGoalId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairGoalVideoMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairGoalVideoMapping_HairGoals_HairGoalId",
                        column: x => x.HairGoalId,
                        principalTable: "HairGoals",
                        principalColumn: "HairGoalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HairGoalVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                        column: x => x.MediaLinkEntityId,
                        principalTable: "MediaLinkEntities",
                        principalColumn: "MediaLinkEntityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HairChallengeVideoMapping_HairChallengeId",
                table: "HairChallengeVideoMapping",
                column: "HairChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_HairChallengeVideoMapping_MediaLinkEntityId",
                table: "HairChallengeVideoMapping",
                column: "MediaLinkEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_HairGoalVideoMapping_HairGoalId",
                table: "HairGoalVideoMapping",
                column: "HairGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_HairGoalVideoMapping_MediaLinkEntityId",
                table: "HairGoalVideoMapping",
                column: "MediaLinkEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HairChallengeVideoMapping");

            migrationBuilder.DropTable(
                name: "HairGoalVideoMapping");
        }
    }
}
