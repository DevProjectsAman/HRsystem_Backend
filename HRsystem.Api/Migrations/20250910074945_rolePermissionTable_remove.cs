using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class rolePermissionTable_remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "AspRolePermissions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 49, 45, 48, DateTimeKind.Local).AddTicks(4340), new DateTime(2025, 9, 10, 7, 49, 45, 48, DateTimeKind.Utc).AddTicks(7284), new Guid("5e98f03f-945c-46e0-a3a3-a52e6e6c4987") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AspRolePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 47, 7, 310, DateTimeKind.Local).AddTicks(9251), new DateTime(2025, 9, 10, 7, 47, 7, 311, DateTimeKind.Utc).AddTicks(3166), new Guid("6d3dfe95-acc0-4f39-992f-955a67e9e72d") });
        }
    }
}
