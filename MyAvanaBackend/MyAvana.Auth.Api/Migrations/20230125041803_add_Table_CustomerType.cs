using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_Table_CustomerType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerTypeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerTypeId",
                table: "AspNetUsers",
                column: "CustomerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CustomerTypes_CustomerTypeId",
                table: "AspNetUsers",
                column: "CustomerTypeId",
                principalTable: "CustomerTypes",
                principalColumn: "CustomerTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CustomerTypes_CustomerTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomerTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerTypeId",
                table: "AspNetUsers");
        }
    }
}
