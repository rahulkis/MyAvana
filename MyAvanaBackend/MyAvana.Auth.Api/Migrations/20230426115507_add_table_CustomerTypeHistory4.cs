using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAvana.Auth.Api.Migrations
{
    public partial class add_table_CustomerTypeHistory4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUser",
                table: "CustomerTypeHistorys");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUser",
                table: "CustomerTypeHistorys",
                nullable: false,
                defaultValue: 0);
        }
    }
}
