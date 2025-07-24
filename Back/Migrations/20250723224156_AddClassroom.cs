using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Guid>>(
                name: "pre_requisites",
                schema: "syki",
                table: "course_curriculums__disciplines",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "classrooms",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classrooms", x => x.id);
                    table.ForeignKey(
                        name: "fk_classrooms_campi_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_classrooms_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classrooms__classes",
                schema: "syki",
                columns: table => new
                {
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classrooms__classes", x => new { x.classroom_id, x.class_id });
                    table.ForeignKey(
                        name: "fk_classrooms__classes_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_classrooms__classes_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalSchema: "syki",
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_classrooms_campus_id",
                schema: "syki",
                table: "classrooms",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_classrooms_institution_id",
                schema: "syki",
                table: "classrooms",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_classrooms__classes_class_id",
                schema: "syki",
                table: "classrooms__classes",
                column: "class_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classrooms__classes",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "classrooms",
                schema: "syki");

            migrationBuilder.DropColumn(
                name: "pre_requisites",
                schema: "syki",
                table: "course_curriculums__disciplines");
        }
    }
}
