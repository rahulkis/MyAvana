﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addmappingtableforscalpchallenges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypeScalpChallenge",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductTypeId = table.Column<int>(nullable: false),
                    HairScalpChallengeId = table.Column<int>(nullable: false),
                    HairChallengesHairChallengeId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypeScalpChallenge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypeScalpChallenge_HairChallenges_HairChallengesHairChallengeId",
                        column: x => x.HairChallengesHairChallengeId,
                        principalTable: "HairChallenges",
                        principalColumn: "HairChallengeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTypeScalpChallenge_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeScalpChallenge_HairChallengesHairChallengeId",
                table: "ProductTypeScalpChallenge",
                column: "HairChallengesHairChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypeScalpChallenge_ProductTypeId",
                table: "ProductTypeScalpChallenge",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypeScalpChallenge");
        }
    }
}