using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class updated_table_CustomerTypeHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypeHistorys_WebLogins_UserId",
                table: "CustomerTypeHistorys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTypeHistorys",
                table: "CustomerTypeHistorys");

            migrationBuilder.RenameTable(
                name: "CustomerTypeHistorys",
                newName: "CustomerTypeHistory");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTypeHistorys_UserId",
                table: "CustomerTypeHistory",
                newName: "IX_CustomerTypeHistory_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CustomerTypeHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTypeHistory",
                table: "CustomerTypeHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UserId",
                table: "CustomerTypeHistory",
                column: "UserId",
                principalTable: "WebLogins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypeHistory_WebLogins_UserId",
                table: "CustomerTypeHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTypeHistory",
                table: "CustomerTypeHistory");

            migrationBuilder.RenameTable(
                name: "CustomerTypeHistory",
                newName: "CustomerTypeHistorys");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTypeHistory_UserId",
                table: "CustomerTypeHistorys",
                newName: "IX_CustomerTypeHistorys_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CustomerTypeHistorys",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTypeHistorys",
                table: "CustomerTypeHistorys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypeHistorys_WebLogins_UserId",
                table: "CustomerTypeHistorys",
                column: "UserId",
                principalTable: "WebLogins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
