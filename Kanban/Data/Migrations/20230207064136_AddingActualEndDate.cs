using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingActualEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "Tasks",
                type: "date",
                nullable: false,
                defaultValue: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "Tasks");
        }
    }
}
