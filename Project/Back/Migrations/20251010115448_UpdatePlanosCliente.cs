using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanosCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_planos_clientes_planos_credito_planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.DropForeignKey(
                name: "fk_planos_clientes_planos_relatorio_planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.DropIndex(
                name: "ix_planos_clientes_planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.DropIndex(
                name: "ix_planos_clientes_planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.DropColumn(
                name: "planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.DropColumn(
                name: "planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.AddColumn<short>(
                name: "billing_period_start_day",
                schema: "faturamento",
                table: "planos_clientes",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billing_period_start_day",
                schema: "faturamento",
                table: "planos_clientes");

            migrationBuilder.AddColumn<int>(
                name: "planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_creditos_id");

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_relatorios_id");

            migrationBuilder.AddForeignKey(
                name: "fk_planos_clientes_planos_credito_planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_creditos_id",
                principalSchema: "faturamento",
                principalTable: "planos_creditos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_planos_clientes_planos_relatorio_planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_relatorios_id",
                principalSchema: "faturamento",
                principalTable: "planos_relatorios",
                principalColumn: "id");
        }
    }
}
