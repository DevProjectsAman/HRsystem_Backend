using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class rolePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "AspRolePermissions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 9, 12, 54, 53, 907, DateTimeKind.Local).AddTicks(6699), new DateTime(2025, 9, 9, 9, 54, 53, 908, DateTimeKind.Utc).AddTicks(1479), new Guid("249f59fe-aed6-4e69-bb8b-9c5856718bb8") });
        }
    }
}
