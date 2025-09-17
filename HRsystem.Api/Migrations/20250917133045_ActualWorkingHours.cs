using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class ActualWorkingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualWorkingHours",
                table: "Tb_Employee_Attendance",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 16, 30, 44, 17, DateTimeKind.Local).AddTicks(9285), new DateTime(2025, 9, 17, 13, 30, 44, 18, DateTimeKind.Utc).AddTicks(2427), new Guid("51dc9f7d-8a35-4b2a-afc8-95e3241e0475") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualWorkingHours",
                table: "Tb_Employee_Attendance");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 49, 4, 256, DateTimeKind.Local).AddTicks(9388), new DateTime(2025, 9, 17, 11, 49, 4, 257, DateTimeKind.Utc).AddTicks(7138), new Guid("c4baeba0-6544-44ad-9b90-746d40c2348e") });
        }
    }
}
