using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherDiscipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_classes_teachers_teacher_id",
                schema: "syki",
                table: "classes");

            migrationBuilder.AlterColumn<Guid>(
                name: "teacher_id",
                schema: "syki",
                table: "classes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "campus_id",
                schema: "syki",
                table: "classes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "teachers__disciplines",
                schema: "syki",
                columns: table => new
                {
                    syki_teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers__disciplines", x => new { x.syki_teacher_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_teachers__disciplines_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachers__disciplines_teachers_syki_teacher_id",
                        column: x => x.syki_teacher_id,
                        principalSchema: "syki",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_classes_campus_id",
                schema: "syki",
                table: "classes",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_teachers__disciplines_discipline_id",
                schema: "syki",
                table: "teachers__disciplines",
                column: "discipline_id");

            migrationBuilder.AddForeignKey(
                name: "fk_classes_campi_campus_id",
                schema: "syki",
                table: "classes",
                column: "campus_id",
                principalSchema: "syki",
                principalTable: "campi",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_classes_teachers_teacher_id",
                schema: "syki",
                table: "classes",
                column: "teacher_id",
                principalSchema: "syki",
                principalTable: "teachers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_classes_campi_campus_id",
                schema: "syki",
                table: "classes");

            migrationBuilder.DropForeignKey(
                name: "fk_classes_teachers_teacher_id",
                schema: "syki",
                table: "classes");

            migrationBuilder.DropTable(
                name: "teachers__disciplines",
                schema: "syki");

            migrationBuilder.DropIndex(
                name: "ix_classes_campus_id",
                schema: "syki",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "campus_id",
                schema: "syki",
                table: "classes");

            migrationBuilder.AlterColumn<Guid>(
                name: "teacher_id",
                schema: "syki",
                table: "classes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_classes_teachers_teacher_id",
                schema: "syki",
                table: "classes",
                column: "teacher_id",
                principalSchema: "syki",
                principalTable: "teachers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
