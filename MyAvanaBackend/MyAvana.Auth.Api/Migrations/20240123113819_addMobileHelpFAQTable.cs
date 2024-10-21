using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addMobileHelpFAQTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MobileHelpFAQ",
                columns: table => new
                {
                    MobileHelpFAQId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Videolink = table.Column<string>(nullable: true),
                    ImageLink = table.Column<string>(nullable: true),
                    VideoThumbnail = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileHelpFAQ", x => x.MobileHelpFAQId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobileHelpFAQ");

           
        }
    }
}
