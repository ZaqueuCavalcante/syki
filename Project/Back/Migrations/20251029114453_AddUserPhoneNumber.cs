using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_phone_numbers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    area_code = table.Column<string>(type: "text", nullable: true),
                    number = table.Column<string>(type: "text", nullable: false),
                    phone_type = table.Column<short>(type: "smallint", nullable: false),
                    country_code = table.Column<int>(type: "integer", nullable: true),
                    original_input = table.Column<string>(type: "text", nullable: true),
                    cliente_id = table.Column<int>(type: "integer", nullable: true),
                    migrated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    migrated_from_cliente_id = table.Column<int>(type: "integer", nullable: true),
                    migrated_from_user_id = table.Column<int>(type: "integer", nullable: true),
                    verified = table.Column<bool>(type: "boolean", nullable: true),
                    verification_date = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_phone_numbers_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_phone_numbers_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id");
                    table.ForeignKey(
                        name: "user_phone_numbers_users_user_id_fkey",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_numbers_cliente_id",
                schema: "public",
                table: "user_phone_numbers",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_phone_numbers_user_id",
                schema: "public",
                table: "user_phone_numbers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_phone_numbers_country_number_idx",
                schema: "public",
                table: "user_phone_numbers",
                columns: new[] { "country_code", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_phone_numbers_ddd_number_idx",
                schema: "public",
                table: "user_phone_numbers",
                columns: new[] { "area_code", "number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_phone_numbers",
                schema: "public");
        }
    }
}
