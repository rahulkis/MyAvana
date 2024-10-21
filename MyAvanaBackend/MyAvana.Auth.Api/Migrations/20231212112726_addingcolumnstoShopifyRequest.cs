using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addingcolumnstoShopifyRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HairAIAvailDate",
                table: "ShopifyRequest",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHairAIAvailed",
                table: "ShopifyRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HairAIAvailDate",
                table: "ShopifyRequest");

            migrationBuilder.DropColumn(
                name: "IsHairAIAvailed",
                table: "ShopifyRequest");
        }
    }
}
