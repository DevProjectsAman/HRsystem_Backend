using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class RulesNames22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VacationRuleName",
                table: "Tb_Vacation_Rule",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 10, 5, 44, 635, DateTimeKind.Local).AddTicks(4182), new DateTime(2025, 11, 12, 10, 5, 44, 635, DateTimeKind.Local).AddTicks(8403), new Guid("23a1807c-d886-4d08-9a88-927c3b0cd04a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tb_Vacation_Rule",
                keyColumn: "VacationRuleName",
                keyValue: null,
                column: "VacationRuleName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "VacationRuleName",
                table: "Tb_Vacation_Rule",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 10, 4, 17, 51, DateTimeKind.Local).AddTicks(3538), new DateTime(2025, 11, 12, 10, 4, 17, 51, DateTimeKind.Local).AddTicks(8321), new Guid("59ca2a9c-d18f-4638-9210-8791be6f40ef") });
        }
    }
}
