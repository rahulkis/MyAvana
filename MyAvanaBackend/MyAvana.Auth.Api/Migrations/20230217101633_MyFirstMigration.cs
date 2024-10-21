using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "WebLogins",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebLogins_UserTypeId",
                table: "WebLogins",
                column: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebLogins_UserTypes_UserTypeId",
                table: "WebLogins",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebLogins_UserTypes_UserTypeId",
                table: "WebLogins");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropIndex(
                name: "IX_WebLogins_UserTypeId",
                table: "WebLogins");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "WebLogins");
        }
    }
}
