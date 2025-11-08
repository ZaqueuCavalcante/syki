using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddFaturamentoValorTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "valor_total_faturamento_id_seq",
                schema: "faturamento");

            migrationBuilder.CreateTable(
                name: "valor_total",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.valor_total_faturamento_id_seq')"),
                    parent_organization_id = table.Column<int>(type: "integer", nullable: false),
                    valor_total_creditos = table.Column<decimal>(type: "numeric", nullable: false),
                    valor_total_relatorios = table.Column<decimal>(type: "numeric", nullable: false),
                    inserido_em = table.Column<DateTime>(type: "timestamp", nullable: false),
                    alterado_em = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ano_mes = table.Column<int>(type: "integer", nullable: true),
                    valor_total_doccheck = table.Column<decimal>(type: "numeric", nullable: true),
                    franquia_minima = table.Column<decimal>(type: "numeric", nullable: true),
                    valor_consumo = table.Column<decimal>(type: "numeric", nullable: true),
                    valor_final = table.Column<decimal>(type: "numeric", nullable: true),
                    nf_group = table.Column<int>(type: "integer", nullable: true),
                    contract_code = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("valor_total_faturamento_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_valor_total_cliente_parent_organization_id",
                        column: x => x.parent_organization_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "unique_parent_anomes_contrato",
                schema: "faturamento",
                table: "valor_total",
                columns: new[] { "parent_organization_id", "ano_mes", "contract_code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "valor_total",
                schema: "faturamento");

            migrationBuilder.DropSequence(
                name: "valor_total_faturamento_id_seq",
                schema: "faturamento");
        }
    }
}
