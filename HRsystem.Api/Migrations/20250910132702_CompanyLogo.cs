using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class CompanyLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "Tb_Company",
                type: "varchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 10, 16, 26, 58, 103, DateTimeKind.Local).AddTicks(823), new DateTime(2025, 9, 10, 13, 26, 58, 103, DateTimeKind.Utc).AddTicks(4399), new Guid("1b920973-bcfd-4ffe-b8ca-1aa0e9cb7838") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "Tb_Company");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 49, 45, 48, DateTimeKind.Local).AddTicks(4340), new DateTime(2025, 9, 10, 7, 49, 45, 48, DateTimeKind.Utc).AddTicks(7284), new Guid("5e98f03f-945c-46e0-a3a3-a52e6e6c4987") });
        }
    }
}
