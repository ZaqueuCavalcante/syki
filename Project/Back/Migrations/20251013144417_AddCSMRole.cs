using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddCSMRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "exato",
                table: "roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[] { new Guid("0199d4cf-133f-7bcb-b37d-06e2c30ebbc9"), "0199d4cf-133f-7bcb-b37d-06e2c30ebbc9", "OfficeCustomerSuccessManager", "OFFICECUSTOMERSUCCESSMANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0199d4cf-133f-7bcb-b37d-06e2c30ebbc9"));
        }
    }
}
