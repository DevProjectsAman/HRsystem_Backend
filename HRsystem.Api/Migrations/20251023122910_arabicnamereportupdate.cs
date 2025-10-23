using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class arabicnamereportupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnglishFullName",
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicFullName",
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 23, 15, 29, 7, 320, DateTimeKind.Local).AddTicks(3244), new DateTime(2025, 10, 23, 12, 29, 7, 320, DateTimeKind.Utc).AddTicks(6489), new Guid("5b3acde3-2d00-4a7b-ac05-7a835f0ea876") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tb_Employee_Monthly_Report",
                keyColumn: "EnglishFullName",
                keyValue: null,
                column: "EnglishFullName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishFullName",
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee_Monthly_Report",
                keyColumn: "ArabicFullName",
                keyValue: null,
                column: "ArabicFullName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicFullName",
                table: "Tb_Employee_Monthly_Report",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 22, 16, 8, 20, 754, DateTimeKind.Local).AddTicks(8072), new DateTime(2025, 10, 22, 13, 8, 20, 755, DateTimeKind.Utc).AddTicks(1798), new Guid("2216268c-a6e9-4d5f-92cb-1cf0ed621f19") });
        }
    }
}
