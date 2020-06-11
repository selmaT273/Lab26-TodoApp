using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab26_TodoApp.Migrations
{
    public partial class addingcreatedbyuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Todos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Todos");
        }
    }
}
