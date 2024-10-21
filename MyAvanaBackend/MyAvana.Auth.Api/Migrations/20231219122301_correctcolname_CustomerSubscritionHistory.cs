using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class correctcolname_CustomerSubscritionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HairAIAvailDate",
                table: "CustomerSubscriptionHistory",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CustomerSubscriptionHistory",
                newName: "HairAIAvailDate");
        }
    }
}
