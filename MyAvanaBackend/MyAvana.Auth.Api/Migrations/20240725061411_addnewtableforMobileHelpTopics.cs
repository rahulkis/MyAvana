using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addnewtableforMobileHelpTopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileHelpTopicId",
                table: "MobileHelpFAQ",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MobileHelpTopic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileHelpTopic", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MobileHelpFAQ_MobileHelpTopicId",
                table: "MobileHelpFAQ",
                column: "MobileHelpTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_MobileHelpFAQ_MobileHelpTopic_MobileHelpTopicId",
                table: "MobileHelpFAQ",
                column: "MobileHelpTopicId",
                principalTable: "MobileHelpTopic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobileHelpFAQ_MobileHelpTopic_MobileHelpTopicId",
                table: "MobileHelpFAQ");

            migrationBuilder.DropTable(
                name: "MobileHelpTopic");

            migrationBuilder.DropIndex(
                name: "IX_MobileHelpFAQ_MobileHelpTopicId",
                table: "MobileHelpFAQ");

            migrationBuilder.DropColumn(
                name: "MobileHelpTopicId",
                table: "MobileHelpFAQ");
        }
    }
}
