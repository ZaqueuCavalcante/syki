using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleClassroomId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "classroom_id",
                schema: "syki",
                table: "schedules",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_schedules_classroom_id",
                schema: "syki",
                table: "schedules",
                column: "classroom_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedules_classrooms_classroom_id",
                schema: "syki",
                table: "schedules",
                column: "classroom_id",
                principalSchema: "syki",
                principalTable: "classrooms",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedules_classrooms_classroom_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.DropIndex(
                name: "ix_schedules_classroom_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "classroom_id",
                schema: "syki",
                table: "schedules");
        }
    }
}
