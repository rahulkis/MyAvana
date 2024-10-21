using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class updatetableProductTypeScalpChallenge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypeScalpChallenge_HairChallenges_HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypeScalpChallenge_HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge");

            migrationBuilder.DropColumn(
                name: "HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeScalpChallenge_HairScalpChallengeId",
                table: "ProductTypeScalpChallenge",
                column: "HairScalpChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypeScalpChallenge_HairChallenges_HairScalpChallengeId",
                table: "ProductTypeScalpChallenge",
                column: "HairScalpChallengeId",
                principalTable: "HairChallenges",
                principalColumn: "HairChallengeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypeScalpChallenge_HairChallenges_HairScalpChallengeId",
                table: "ProductTypeScalpChallenge");

            migrationBuilder.DropIndex(
                name: "IX_ProductTypeScalpChallenge_HairScalpChallengeId",
                table: "ProductTypeScalpChallenge");

            migrationBuilder.AddColumn<int>(
                name: "HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeScalpChallenge_HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge",
                column: "HairChallengesHairChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypeScalpChallenge_HairChallenges_HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge",
                column: "HairChallengesHairChallengeId",
                principalTable: "HairChallenges",
                principalColumn: "HairChallengeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
