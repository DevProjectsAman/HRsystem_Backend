using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeCodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MobileApplicationVersion",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "MobileApplicationVersion", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 9, 17, 7, 628, DateTimeKind.Utc).AddTicks(5471), new DateTime(2026, 1, 4, 9, 17, 7, 628, DateTimeKind.Utc).AddTicks(8650), "1.0.0", new Guid("0247d216-ed0f-4fb9-9d4d-5b0f5576e294") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileApplicationVersion",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "SerialMobile",
                keyValue: null,
                column: "SerialMobile",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SerialMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 29, 11, 1, 36, 815, DateTimeKind.Utc).AddTicks(9210), new DateTime(2025, 12, 29, 11, 1, 36, 816, DateTimeKind.Utc).AddTicks(2143), new Guid("4a4a8c4a-40ec-43d3-a7ee-7363ca4f109e") });
        }
    }
}
