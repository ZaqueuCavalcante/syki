using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherSchedulingPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedules_classes_class_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.AlterColumn<Guid>(
                name: "class_id",
                schema: "syki",
                table: "schedules",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "teacher_id",
                schema: "syki",
                table: "schedules",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_schedules_teacher_id",
                schema: "syki",
                table: "schedules",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedules_classes_class_id",
                schema: "syki",
                table: "schedules",
                column: "class_id",
                principalSchema: "syki",
                principalTable: "classes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_schedules_teachers_teacher_id",
                schema: "syki",
                table: "schedules",
                column: "teacher_id",
                principalSchema: "syki",
                principalTable: "teachers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_schedules_classes_class_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.DropForeignKey(
                name: "fk_schedules_teachers_teacher_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.DropIndex(
                name: "ix_schedules_teacher_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.DropColumn(
                name: "teacher_id",
                schema: "syki",
                table: "schedules");

            migrationBuilder.AlterColumn<Guid>(
                name: "class_id",
                schema: "syki",
                table: "schedules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_schedules_classes_class_id",
                schema: "syki",
                table: "schedules",
                column: "class_id",
                principalSchema: "syki",
                principalTable: "classes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
