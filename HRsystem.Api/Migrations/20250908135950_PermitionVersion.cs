using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class PermitionVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermissionVersion",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EmployeeId", "LastPasswordChangedAt", "PermissionVersion", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 8, 16, 59, 49, 439, DateTimeKind.Local).AddTicks(8011), 1, new DateTime(2025, 9, 8, 13, 59, 49, 440, DateTimeKind.Utc).AddTicks(1156), 1, new Guid("57c3cc3a-1070-4555-9db7-78e0f25e7279") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PermissionVersion",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 8, 13, 3, 38, 309, DateTimeKind.Local).AddTicks(6082), new DateTime(2025, 9, 8, 10, 3, 38, 309, DateTimeKind.Utc).AddTicks(8818), new Guid("fcac65de-74c4-4d49-acd7-4fa50e02ee85") });
        }
    }
}
