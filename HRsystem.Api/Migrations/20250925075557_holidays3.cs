using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class holidays3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tb_holidays",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 25, 10, 55, 57, 42, DateTimeKind.Local).AddTicks(4576), new DateTime(2025, 9, 25, 7, 55, 57, 42, DateTimeKind.Utc).AddTicks(7695), new Guid("001e83a2-64d2-4eda-a721-419666c3c864") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tb_holidays");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 25, 10, 52, 51, 275, DateTimeKind.Local).AddTicks(8586), new DateTime(2025, 9, 25, 7, 52, 51, 276, DateTimeKind.Utc).AddTicks(1778), new Guid("aa364ee7-d004-4d70-87f5-9280549109b3") });
        }
    }
}
