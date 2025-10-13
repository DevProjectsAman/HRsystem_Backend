using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class employeeattendanceupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttStatues",
                table: "Tb_Employee_Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 13, 17, 15, 34, 4, DateTimeKind.Local).AddTicks(6461), new DateTime(2025, 10, 13, 14, 15, 34, 5, DateTimeKind.Utc).AddTicks(1), new Guid("0cc61777-5d87-46cd-b06b-4c31e14e1a7b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttStatues",
                table: "Tb_Employee_Attendance");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 1, 16, 43, 14, 506, DateTimeKind.Local).AddTicks(9165), new DateTime(2025, 10, 1, 13, 43, 14, 507, DateTimeKind.Utc).AddTicks(2321), new Guid("9dcc46c7-c0a8-425f-8480-fd49a48cf541") });
        }
    }
}
