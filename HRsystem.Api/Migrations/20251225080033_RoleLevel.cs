using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class RoleLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleLevel",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleLevel",
                value: 1000);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 25, 8, 0, 30, 38, DateTimeKind.Utc).AddTicks(3307), new DateTime(2025, 12, 25, 8, 0, 30, 38, DateTimeKind.Utc).AddTicks(6429), new Guid("ede85ed7-ad3e-42f0-a524-c51d0154ade1") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleLevel",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 24, 19, 4, 13, 776, DateTimeKind.Utc).AddTicks(2564), new DateTime(2025, 12, 24, 19, 4, 13, 776, DateTimeKind.Utc).AddTicks(5631), new Guid("ed2006e3-b935-44be-b960-dbe7e8e05535") });
        }
    }
}
