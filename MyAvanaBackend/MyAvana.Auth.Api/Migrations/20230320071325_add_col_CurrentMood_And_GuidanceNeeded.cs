using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_col_CurrentMood_And_GuidanceNeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentMood",
                table: "DailyRoutineTracker",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuidanceNeeded",
                table: "DailyRoutineTracker",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMood",
                table: "DailyRoutineTracker");

            migrationBuilder.DropColumn(
                name: "GuidanceNeeded",
                table: "DailyRoutineTracker");
        }
    }
}
