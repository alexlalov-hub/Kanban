using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Statuse",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statuse",
                table: "Tasks");
        }
    }
}
