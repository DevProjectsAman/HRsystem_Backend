using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class Punch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PunchType",
                table: "Tb_Employee_Attendance_Punch",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 49, 4, 256, DateTimeKind.Local).AddTicks(9388), new DateTime(2025, 9, 17, 11, 49, 4, 257, DateTimeKind.Utc).AddTicks(7138), new Guid("c4baeba0-6544-44ad-9b90-746d40c2348e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PunchType",
                table: "Tb_Employee_Attendance_Punch",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 13, 58, 52, 143, DateTimeKind.Local).AddTicks(5370), new DateTime(2025, 9, 17, 10, 58, 52, 143, DateTimeKind.Utc).AddTicks(8521), new Guid("bb35a945-1597-4f9a-8105-ac7d6e522bb3") });
        }
    }
}
