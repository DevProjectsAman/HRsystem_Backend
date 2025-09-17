using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class PunchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PunchIn",
                table: "Tb_Employee_Attendance_Punch");

            migrationBuilder.RenameColumn(
                name: "PunchOut",
                table: "Tb_Employee_Attendance_Punch",
                newName: "PunchTime");

            migrationBuilder.AddColumn<string>(
                name: "PunchType",
                table: "Tb_Employee_Attendance_Punch",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AttendanceDate",
                table: "Tb_Employee_Attendance",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 13, 58, 52, 143, DateTimeKind.Local).AddTicks(5370), new DateTime(2025, 9, 17, 10, 58, 52, 143, DateTimeKind.Utc).AddTicks(8521), new Guid("bb35a945-1597-4f9a-8105-ac7d6e522bb3") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PunchType",
                table: "Tb_Employee_Attendance_Punch");

            migrationBuilder.RenameColumn(
                name: "PunchTime",
                table: "Tb_Employee_Attendance_Punch",
                newName: "PunchOut");

            migrationBuilder.AddColumn<DateTime>(
                name: "PunchIn",
                table: "Tb_Employee_Attendance_Punch",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "AttendanceDate",
                table: "Tb_Employee_Attendance",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 16, 11, 47, 23, 181, DateTimeKind.Local).AddTicks(133), new DateTime(2025, 9, 16, 8, 47, 23, 181, DateTimeKind.Utc).AddTicks(2932), new Guid("e0f15d25-34a9-4912-bf58-f83855a2585d") });
        }
    }
}
