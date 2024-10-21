using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class update_col_IsPayment_IsPaid_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPayment",
                table: "AspNetUsers",
                newName: "IsPaid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPaid",
                table: "AspNetUsers",
                newName: "IsPayment");
        }
    }
}
