using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeCodesTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocFullPath",
                table: "Tb_EmployeeCode_Tracking",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 10, 39, 31, 878, DateTimeKind.Utc).AddTicks(6223), new DateTime(2026, 1, 4, 10, 39, 31, 878, DateTimeKind.Utc).AddTicks(9310), new Guid("663b41fc-f13e-4e83-aa85-cf87ed27b162") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocFullPath",
                table: "Tb_EmployeeCode_Tracking");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 9, 20, 57, 174, DateTimeKind.Utc).AddTicks(9305), new DateTime(2026, 1, 4, 9, 20, 57, 175, DateTimeKind.Utc).AddTicks(2596), new Guid("33d986d6-2ff2-4e6b-9d6d-54bb0dd2b29f") });
        }
    }
}
