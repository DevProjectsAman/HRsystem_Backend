using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeFinance",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UniqueEmployeeCode",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 11, 16, 18, 558, DateTimeKind.Utc).AddTicks(6613), new DateTime(2026, 1, 4, 11, 16, 18, 559, DateTimeKind.Utc).AddTicks(1176), new Guid("36d98634-67f5-42bb-b53f-d6941d042354") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueEmployeeCode",
                table: "Tb_Employee");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "EmployeeCodeHr",
                keyValue: null,
                column: "EmployeeCodeHr",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "EmployeeCodeFinance",
                keyValue: null,
                column: "EmployeeCodeFinance",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeFinance",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 10, 39, 31, 878, DateTimeKind.Utc).AddTicks(6223), new DateTime(2026, 1, 4, 10, 39, 31, 878, DateTimeKind.Utc).AddTicks(9310), new Guid("663b41fc-f13e-4e83-aa85-cf87ed27b162") });
        }
    }
}
