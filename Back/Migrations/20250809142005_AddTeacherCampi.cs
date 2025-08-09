using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherCampi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "teachers__campi",
                schema: "syki",
                columns: table => new
                {
                    syki_teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers__campi", x => new { x.syki_teacher_id, x.campus_id });
                    table.ForeignKey(
                        name: "fk_teachers__campi_campi_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachers__campi_teachers_syki_teacher_id",
                        column: x => x.syki_teacher_id,
                        principalSchema: "syki",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_teachers__campi_campus_id",
                schema: "syki",
                table: "teachers__campi",
                column: "campus_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "teachers__campi",
                schema: "syki");
        }
    }
}
