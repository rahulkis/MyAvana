using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class updated_table_CustomerTypeHistory_UpdatedByUserId_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UpdatedByUserId",
                table: "CustomerTypeHistory");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedByUserId",
                table: "CustomerTypeHistory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UpdatedByUserId",
                table: "CustomerTypeHistory",
                column: "UpdatedByUserId",
                principalTable: "WebLogins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UpdatedByUserId",
                table: "CustomerTypeHistory");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedByUserId",
                table: "CustomerTypeHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UpdatedByUserId",
                table: "CustomerTypeHistory",
                column: "UpdatedByUserId",
                principalTable: "WebLogins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
