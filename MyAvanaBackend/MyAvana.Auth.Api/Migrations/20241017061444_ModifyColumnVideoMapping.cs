using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class ModifyColumnVideoMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairChallengeVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairChallengeVideoMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_HairGoalVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairGoalVideoMapping");

            migrationBuilder.DropColumn(
                name: "MediaLinkEntitiesId",
                table: "HairGoalVideoMapping");

            migrationBuilder.DropColumn(
                name: "MediaLinkEntitiesId",
                table: "HairChallengeVideoMapping");

            migrationBuilder.AlterColumn<int>(
                name: "MediaLinkEntityId",
                table: "HairGoalVideoMapping",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MediaLinkEntityId",
                table: "HairChallengeVideoMapping",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HairChallengeVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairChallengeVideoMapping",
                column: "MediaLinkEntityId",
                principalTable: "MediaLinkEntities",
                principalColumn: "MediaLinkEntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HairGoalVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairGoalVideoMapping",
                column: "MediaLinkEntityId",
                principalTable: "MediaLinkEntities",
                principalColumn: "MediaLinkEntityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HairChallengeVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairChallengeVideoMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_HairGoalVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairGoalVideoMapping");

            migrationBuilder.AlterColumn<int>(
                name: "MediaLinkEntityId",
                table: "HairGoalVideoMapping",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MediaLinkEntitiesId",
                table: "HairGoalVideoMapping",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MediaLinkEntityId",
                table: "HairChallengeVideoMapping",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MediaLinkEntitiesId",
                table: "HairChallengeVideoMapping",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_HairChallengeVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairChallengeVideoMapping",
                column: "MediaLinkEntityId",
                principalTable: "MediaLinkEntities",
                principalColumn: "MediaLinkEntityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HairGoalVideoMapping_MediaLinkEntities_MediaLinkEntityId",
                table: "HairGoalVideoMapping",
                column: "MediaLinkEntityId",
                principalTable: "MediaLinkEntities",
                principalColumn: "MediaLinkEntityId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
