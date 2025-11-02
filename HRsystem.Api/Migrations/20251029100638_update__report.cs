using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class update__report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 29, 13, 6, 32, 44, DateTimeKind.Local).AddTicks(6772), new DateTime(2025, 10, 29, 10, 6, 32, 45, DateTimeKind.Utc).AddTicks(33), new Guid("2a5c3618-9e44-41ed-8427-4e55e2307d45") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 23, 16, 6, 55, 700, DateTimeKind.Local).AddTicks(9983), new DateTime(2025, 10, 23, 13, 6, 55, 701, DateTimeKind.Utc).AddTicks(3609), new Guid("27354276-ae1b-4a71-aed1-6e1691877f8c") });
        }
    }
}
