using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addnewTableForSharedHHCP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedHHCP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairProfileId = table.Column<int>(nullable: false),
                    SharedBy = table.Column<Guid>(nullable: false),
                    SharedWith = table.Column<Guid>(nullable: false),
                    SharedOn = table.Column<DateTime>(nullable: false),
                    RevokedOn = table.Column<DateTime>(nullable: false),
                    IsRevoked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedHHCP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedHHCP_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedHHCP_HairProfileId",
                table: "SharedHHCP",
                column: "HairProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedHHCP");
        }
    }
}
