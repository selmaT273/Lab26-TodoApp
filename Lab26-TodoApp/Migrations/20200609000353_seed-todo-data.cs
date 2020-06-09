using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab26_TodoApp.Migrations
{
    public partial class seedtododata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Assignee", "Difficulty", "DueDate", "Title" },
                values: new object[] { 1, "Stacey", 5, new DateTime(2020, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taxes" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Assignee", "Difficulty", "DueDate", "Title" },
                values: new object[] { 2, "Stacey2", 2, new DateTime(2020, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mail gift" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
