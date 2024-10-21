using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addingIsActiveColumninDiscountCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "DiscountCodes",
                newName: "DiscountPercent");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DiscountCodes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DiscountCodes");

            migrationBuilder.RenameColumn(
                name: "DiscountPercent",
                table: "DiscountCodes",
                newName: "DiscountAmount");
        }
    }
}
