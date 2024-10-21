using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addinghairscopetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "HairScope",
                columns: table => new
                {
                    HairScopeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    HairProfileId = table.Column<int>(nullable: true),
                    QAVersion = table.Column<int>(nullable: true),
                    HairScopeResult = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairScope", x => x.HairScopeId);
                    table.ForeignKey(
                        name: "FK_HairScope_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HairScope_HairProfileId",
                table: "HairScope",
                column: "HairProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HairScope");

          
        }
    }
}
