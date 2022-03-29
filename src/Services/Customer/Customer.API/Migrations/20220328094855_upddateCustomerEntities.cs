using Microsoft.EntityFrameworkCore.Migrations;

namespace Customer.API.Migrations
{
    public partial class upddateCustomerEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccountActive",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountActive",
                table: "Customer");
        }
    }
}
