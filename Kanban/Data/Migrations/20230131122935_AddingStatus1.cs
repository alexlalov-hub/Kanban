using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingStatus1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Statuse",
                table: "Tasks",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "Statuse");
        }
    }
}
