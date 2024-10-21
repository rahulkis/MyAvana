using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addnewTableforProductTypeHairStyle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "HairGoals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "HairChallenges",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductTypeHairStyles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductTypeId = table.Column<int>(nullable: false),
                    HairStyleId = table.Column<int>(nullable: false),
                    HairStylesId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeHairStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairStyles_HairStyles_HairStylesId",
                        column: x => x.HairStylesId,
                        principalTable: "HairStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTypeHairStyles_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairStyles_HairStylesId",
                table: "ProductTypeHairStyles",
                column: "HairStylesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeHairStyles_ProductTypeId",
                table: "ProductTypeHairStyles",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypeHairStyles");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "HairGoals");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "HairChallenges");

           
        }
    }
}
