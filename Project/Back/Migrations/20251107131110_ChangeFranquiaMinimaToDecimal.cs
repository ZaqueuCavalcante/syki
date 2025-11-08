using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFranquiaMinimaToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "franquia_minima",
                schema: "faturamento",
                table: "cliente_config",
                type: "numeric(10,3)",
                precision: 10,
                scale: 3,
                nullable: true,
                defaultValueSql: "495.000",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValue: 495);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "franquia_minima",
                schema: "faturamento",
                table: "cliente_config",
                type: "integer",
                nullable: true,
                defaultValue: 495,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,3)",
                oldPrecision: 10,
                oldScale: 3,
                oldNullable: true,
                oldDefaultValueSql: "495.000");
        }
    }
}
