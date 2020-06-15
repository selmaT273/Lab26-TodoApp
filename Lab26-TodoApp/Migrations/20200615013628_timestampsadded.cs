using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab26_TodoApp.Migrations
{
    public partial class timestampsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedByTimestamp",
                table: "Todos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedByTimestamp",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedByUserId",
                table: "Todos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByTimestamp",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ModifiedByTimestamp",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "Todos");
        }
    }
}
