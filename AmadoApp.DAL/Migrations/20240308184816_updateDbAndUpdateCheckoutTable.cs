using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmadoApp.DAL.Migrations
{
    public partial class updateDbAndUpdateCheckoutTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Checkouts");
        }
    }
}
