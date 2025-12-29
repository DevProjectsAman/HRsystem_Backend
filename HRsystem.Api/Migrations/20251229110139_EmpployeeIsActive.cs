using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmpployeeIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tb_Employee",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 29, 11, 1, 36, 815, DateTimeKind.Utc).AddTicks(9210), new DateTime(2025, 12, 29, 11, 1, 36, 816, DateTimeKind.Utc).AddTicks(2143), new Guid("4a4a8c4a-40ec-43d3-a7ee-7363ca4f109e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tb_Employee");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 28, 11, 26, 41, 902, DateTimeKind.Utc).AddTicks(8421), new DateTime(2025, 12, 28, 11, 26, 41, 903, DateTimeKind.Utc).AddTicks(1525), new Guid("ebd8c46e-1af0-41e1-9a86-b930e38075a9") });
        }
    }
}
