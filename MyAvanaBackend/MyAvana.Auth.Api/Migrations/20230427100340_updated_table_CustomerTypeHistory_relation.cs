using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class updated_table_CustomerTypeHistory_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerTypeHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "CustomerTypeHistory",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTypeHistory_CustomerId",
                table: "CustomerTypeHistory",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTypeHistory_AspNetUsers_CustomerId",
                table: "CustomerTypeHistory",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTypeHistory_AspNetUsers_CustomerId",
                table: "CustomerTypeHistory");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTypeHistory_CustomerId",
                table: "CustomerTypeHistory");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerTypeHistory");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CustomerTypeHistory",
                nullable: true);
        }
    }
}
