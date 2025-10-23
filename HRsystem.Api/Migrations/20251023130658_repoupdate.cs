using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class repoupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WorkDaysId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JobLevelId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee_Monthly_Report",
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
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ContractTypeId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 23, 16, 6, 55, 700, DateTimeKind.Local).AddTicks(9983), new DateTime(2025, 10, 23, 13, 6, 55, 701, DateTimeKind.Utc).AddTicks(3609), new Guid("27354276-ae1b-4a71-aed1-6e1691877f8c") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WorkDaysId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobLevelId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee_Monthly_Report",
                keyColumn: "EmployeeCodeHr",
                keyValue: null,
                column: "EmployeeCodeHr",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee_Monthly_Report",
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
                table: "Tb_Employee_Monthly_Report",
                keyColumn: "EmployeeCodeFinance",
                keyValue: null,
                column: "EmployeeCodeFinance",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeFinance",
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractTypeId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Tb_Employee_Monthly_Report",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 23, 15, 29, 7, 320, DateTimeKind.Local).AddTicks(3244), new DateTime(2025, 10, 23, 12, 29, 7, 320, DateTimeKind.Utc).AddTicks(6489), new Guid("5b3acde3-2d00-4a7b-ac05-7a835f0ea876") });
        }
    }
}
