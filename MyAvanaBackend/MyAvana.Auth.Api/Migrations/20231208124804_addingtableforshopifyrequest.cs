using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addingtableforshopifyrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHairAIAllowed",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopifyRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Payload = table.Column<string>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    IsExistingCustomer = table.Column<bool>(nullable: true),
                    SubscriptionType = table.Column<int>(nullable: true),
                    AlreadyActiveSubscription = table.Column<bool>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CustomerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopifyRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopifyRequest_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopifyRequest_CustomerId",
                table: "ShopifyRequest",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopifyRequest");

            migrationBuilder.DropColumn(
                name: "IsHairAIAllowed",
                table: "AspNetUsers");
        }
    }
}
