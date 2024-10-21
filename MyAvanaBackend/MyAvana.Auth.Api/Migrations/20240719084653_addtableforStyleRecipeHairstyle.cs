using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class addtableforStyleRecipeHairstyle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StyleRecipeHairStyle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HairStyleId = table.Column<int>(nullable: false),
                    HairProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StyleRecipeHairStyle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StyleRecipeHairStyle_HairProfiles_HairProfileId",
                        column: x => x.HairProfileId,
                        principalTable: "HairProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StyleRecipeHairStyle_HairProfileId",
                table: "StyleRecipeHairStyle",
                column: "HairProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StyleRecipeHairStyle");
        }
    }
}
