using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class AddcolumnExpirationDateToPaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HairAIAvailDate",
                table: "PaymentEntities",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHairAIAvailed",
                table: "PaymentEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "HairAIAvailDate",
                table: "PaymentEntities");

            migrationBuilder.DropColumn(
                name: "IsHairAIAvailed",
                table: "PaymentEntities");
        }
    }
}
