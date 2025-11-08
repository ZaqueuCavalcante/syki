using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class AddExatoRoleProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0199d4cf-133f-7bcb-b37d-06e2c30ebbc9"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("3c076119-c6ca-44c9-86e1-81785664b8b5"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("588188bd-b870-454a-b9e6-5bb005e9a5bf"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("5912ebe1-9e6a-4ce1-90bf-8490534fb4e1"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("61a1cd29-f513-4a25-9be0-f47d6aef90e7"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("78691a7a-f554-42d7-a5cf-8d474b6de0dd"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("7a8ee2ef-d8e7-499e-be2f-967ac20092bf"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("95b27c30-f027-4971-b715-22a2e1f138fe"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("a0acdbb6-eab8-40dd-af9a-e76134dd9445"));

            migrationBuilder.DeleteData(
                schema: "exato",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("d33c08ac-737a-4076-8678-e2cbe157c450"));

            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "exato",
                table: "roles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<int>>(
                name: "features",
                schema: "exato",
                table: "roles",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "organization_id",
                schema: "exato",
                table: "roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_roles_organization_id_normalized_name",
                schema: "exato",
                table: "roles",
                columns: new[] { "organization_id", "normalized_name" },
                unique: true);

            migrationBuilder.Sql("DROP INDEX exato.role_name_index;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_roles_organization_id_normalized_name",
                schema: "exato",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "description",
                schema: "exato",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "features",
                schema: "exato",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "organization_id",
                schema: "exato",
                table: "roles");

            migrationBuilder.InsertData(
                schema: "exato",
                table: "roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("0199d4cf-133f-7bcb-b37d-06e2c30ebbc9"), "0199d4cf-133f-7bcb-b37d-06e2c30ebbc9", "OfficeCustomerSuccessManager", "OFFICECUSTOMERSUCCESSMANAGER" },
                    { new Guid("3c076119-c6ca-44c9-86e1-81785664b8b5"), "3c076119-c6ca-44c9-86e1-81785664b8b5", "OrgFinance", "ORGFINANCE" },
                    { new Guid("588188bd-b870-454a-b9e6-5bb005e9a5bf"), "588188bd-b870-454a-b9e6-5bb005e9a5bf", "OrgRecruiter", "ORGRECRUITER" },
                    { new Guid("5912ebe1-9e6a-4ce1-90bf-8490534fb4e1"), "5912ebe1-9e6a-4ce1-90bf-8490534fb4e1", "OfficeAdm", "OFFICEADM" },
                    { new Guid("61a1cd29-f513-4a25-9be0-f47d6aef90e7"), "61a1cd29-f513-4a25-9be0-f47d6aef90e7", "OfficeSupport", "OFFICESUPPORT" },
                    { new Guid("78691a7a-f554-42d7-a5cf-8d474b6de0dd"), "78691a7a-f554-42d7-a5cf-8d474b6de0dd", "OfficeCustomerSuccess", "OFFICECUSTOMERSUCCESS" },
                    { new Guid("7a8ee2ef-d8e7-499e-be2f-967ac20092bf"), "7a8ee2ef-d8e7-499e-be2f-967ac20092bf", "OrgCandidate", "ORGCANDIDATE" },
                    { new Guid("95b27c30-f027-4971-b715-22a2e1f138fe"), "95b27c30-f027-4971-b715-22a2e1f138fe", "OfficeFinance", "OFFICEFINANCE" },
                    { new Guid("a0acdbb6-eab8-40dd-af9a-e76134dd9445"), "a0acdbb6-eab8-40dd-af9a-e76134dd9445", "OrgAdm", "ORGADM" },
                    { new Guid("d33c08ac-737a-4076-8678-e2cbe157c450"), "d33c08ac-737a-4076-8678-e2cbe157c450", "OrgManager", "ORGMANAGER" }
                });
        }
    }
}
