using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class AddColumnToMappingHairStyleandProductType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "MappingHairStyleandProductType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "MappingHairStyleandProductType");
        }
    }
}
