using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class RenameScheduleProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "start_at",
                schema: "syki",
                table: "schedules",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "end_at",
                schema: "syki",
                table: "schedules",
                newName: "end");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "start",
                schema: "syki",
                table: "schedules",
                newName: "start_at");

            migrationBuilder.RenameColumn(
                name: "end",
                schema: "syki",
                table: "schedules",
                newName: "end_at");
        }
    }
}
