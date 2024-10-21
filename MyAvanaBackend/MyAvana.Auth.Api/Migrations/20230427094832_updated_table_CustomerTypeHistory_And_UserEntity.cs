using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class updated_table_CustomerTypeHistory_And_UserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CustomerTypeHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignupFrom",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerTypeHistory");

            migrationBuilder.DropColumn(
                name: "SignupFrom",
                table: "AspNetUsers");
        }
    }
}
