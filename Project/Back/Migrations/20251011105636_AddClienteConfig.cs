using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "planos_clientes",
                schema: "faturamento");

            migrationBuilder.CreateTable(
                name: "cliente_config",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.customers_plan_package_id_seq')"),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    franquia_minima = table.Column<int>(type: "integer", nullable: true, defaultValue: 495),
                    faturamento_por_faixa = table.Column<bool>(type: "boolean", nullable: false),
                    planos_doccheck_id = table.Column<int>(type: "integer", nullable: true),
                    faturamento_por_rateio = table.Column<bool>(type: "boolean", nullable: false),
                    detalhar_relatorios = table.Column<bool>(type: "boolean", nullable: true),
                    exibir_nao_consumidores = table.Column<bool>(type: "boolean", nullable: true),
                    cliente_contact_id = table.Column<int>(type: "integer", nullable: true),
                    previous_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    unmasked_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    summary_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    v1_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    billing_period_start_day = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_plan_package_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cliente_config_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cliente_config_cliente_contact_cliente_contact_id",
                        column: x => x.cliente_contact_id,
                        principalSchema: "faturamento",
                        principalTable: "cliente_contact",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_cliente_config_cliente_contact_id",
                schema: "faturamento",
                table: "cliente_config",
                column: "cliente_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_config_cliente_id",
                schema: "faturamento",
                table: "cliente_config",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente_config",
                schema: "faturamento");

            migrationBuilder.CreateTable(
                name: "planos_clientes",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.customers_plan_package_id_seq')"),
                    billing_period_start_day = table.Column<short>(type: "smallint", nullable: true),
                    cliente_contact_id = table.Column<int>(type: "integer", nullable: true),
                    cliente_id = table.Column<int>(type: "integer", nullable: true),
                    detalhar_relatorios = table.Column<bool>(type: "boolean", nullable: true),
                    exibir_nao_consumidores = table.Column<bool>(type: "boolean", nullable: true),
                    faturamento_por_faixa = table.Column<bool>(type: "boolean", nullable: false),
                    faturamento_por_rateio = table.Column<bool>(type: "boolean", nullable: true),
                    franquia_minima = table.Column<int>(type: "integer", nullable: true, defaultValue: 495),
                    planos_doccheck_id = table.Column<int>(type: "integer", nullable: true),
                    previous_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    summary_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    unmasked_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    v1_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_plan_package_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_planos_clientes_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id");
                    table.ForeignKey(
                        name: "fk_planos_clientes_cliente_contact_cliente_contact_id",
                        column: x => x.cliente_contact_id,
                        principalSchema: "faturamento",
                        principalTable: "cliente_contact",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_cliente_contact_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "cliente_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_cliente_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "cliente_id");
        }
    }
}
