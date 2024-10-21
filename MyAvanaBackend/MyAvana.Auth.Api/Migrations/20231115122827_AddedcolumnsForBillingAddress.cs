using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class AddedcolumnsForBillingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "PaymentEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "City",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "State",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "PaymentEntities");
        }
    }
}
