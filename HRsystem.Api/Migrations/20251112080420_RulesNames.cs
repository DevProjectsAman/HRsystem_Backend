using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class RulesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkDaysRuleName",
                table: "Tb_WorkDays_Rules",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ShiftRuleName",
                table: "Tb_Shift_Rule",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 10, 4, 17, 51, DateTimeKind.Local).AddTicks(3538), new DateTime(2025, 11, 12, 10, 4, 17, 51, DateTimeKind.Local).AddTicks(8321), new Guid("59ca2a9c-d18f-4638-9210-8791be6f40ef") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkDaysRuleName",
                table: "Tb_WorkDays_Rules");

            migrationBuilder.DropColumn(
                name: "ShiftRuleName",
                table: "Tb_Shift_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 12, 8, 29, 592, DateTimeKind.Local).AddTicks(8054), new DateTime(2025, 11, 11, 12, 8, 29, 593, DateTimeKind.Local).AddTicks(1482), new Guid("74e5ca98-de9f-47e8-bd48-432710640da3") });
        }
    }
}
