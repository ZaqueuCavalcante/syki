using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandActivityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "activity_id",
                schema: "syki",
                table: "commands",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activity_id",
                schema: "syki",
                table: "commands");
        }
    }
}
