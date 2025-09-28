using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 25, 42, 385, DateTimeKind.Local).AddTicks(8822), new DateTime(2025, 9, 28, 10, 25, 42, 386, DateTimeKind.Utc).AddTicks(2647), new Guid("f621b35b-f973-46c1-95e2-caeefbeae7da") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 25, 10, 55, 57, 42, DateTimeKind.Local).AddTicks(4576), new DateTime(2025, 9, 25, 7, 55, 57, 42, DateTimeKind.Utc).AddTicks(7695), new Guid("001e83a2-64d2-4eda-a721-419666c3c864") });
        }
    }
}
