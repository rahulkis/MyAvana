using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class delete_col_SalonNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalonNotes",
                table: "HairProfiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalonNotes",
                table: "HairProfiles",
                nullable: true);
        }
    }
}
