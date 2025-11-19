using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class ContractTypes22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_ContractType",
                table: "Tb_ContractType");

            migrationBuilder.RenameTable(
                name: "Tb_ContractType",
                newName: "Tb_Contract_Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Contract_Type",
                table: "Tb_Contract_Type",
                column: "ContractTypeId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 18, 13, 34, 24, 648, DateTimeKind.Local).AddTicks(2601), new DateTime(2025, 11, 18, 13, 34, 24, 648, DateTimeKind.Local).AddTicks(9266), new Guid("8b16709e-33da-4c95-b169-41e8fea18ea9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Contract_Type",
                table: "Tb_Contract_Type");

            migrationBuilder.RenameTable(
                name: "Tb_Contract_Type",
                newName: "Tb_ContractType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_ContractType",
                table: "Tb_ContractType",
                column: "ContractTypeId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 18, 13, 31, 32, 620, DateTimeKind.Local).AddTicks(6269), new DateTime(2025, 11, 18, 13, 31, 32, 620, DateTimeKind.Local).AddTicks(9469), new Guid("f363391d-6bb9-4d07-aa0c-bbd461a032b4") });
        }
    }
}
