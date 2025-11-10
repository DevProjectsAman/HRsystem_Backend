using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class updatetbattstatues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Attendance_Statues",
                table: "Tb_Attendance_Statues");

            migrationBuilder.RenameTable(
                name: "Tb_Attendance_Statues",
                newName: "tb_attendance_statues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tb_attendance_statues",
                table: "tb_attendance_statues",
                column: "AttendanceStatuesId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 10, 16, 20, 19, 625, DateTimeKind.Local).AddTicks(1378), new DateTime(2025, 11, 10, 14, 20, 19, 625, DateTimeKind.Utc).AddTicks(5049), new Guid("4cc5de59-7862-4909-a393-df251c86adbe") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tb_attendance_statues",
                table: "tb_attendance_statues");

            migrationBuilder.RenameTable(
                name: "tb_attendance_statues",
                newName: "Tb_Attendance_Statues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Attendance_Statues",
                table: "Tb_Attendance_Statues",
                column: "AttendanceStatuesId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 10, 14, 3, 52, 481, DateTimeKind.Local).AddTicks(5457), new DateTime(2025, 11, 10, 12, 3, 52, 482, DateTimeKind.Utc).AddTicks(1178), new Guid("30c862d6-8660-4f5b-b404-3384ee4d9f0c") });
        }
    }
}
