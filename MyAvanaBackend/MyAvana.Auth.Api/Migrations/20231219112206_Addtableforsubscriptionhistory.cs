using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class Addtableforsubscriptionhistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopifyRequest_AspNetUsers_CustomerId",
                table: "ShopifyRequest");

            migrationBuilder.DropColumn(
                name: "HairAIAvailDate",
                table: "ShopifyRequest");

            migrationBuilder.DropColumn(
                name: "IsHairAIAvailed",
                table: "ShopifyRequest");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "ShopifyRequest",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "CustomerSubscriptionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailAddress = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    HairAIAvailDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSubscriptionHistory", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ShopifyRequest_AspNetUsers_CustomerId",
                table: "ShopifyRequest",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopifyRequest_AspNetUsers_CustomerId",
                table: "ShopifyRequest");

            migrationBuilder.DropTable(
                name: "CustomerSubscriptionHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "ShopifyRequest",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HairAIAvailDate",
                table: "ShopifyRequest",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHairAIAvailed",
                table: "ShopifyRequest",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopifyRequest_AspNetUsers_CustomerId",
                table: "ShopifyRequest",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
