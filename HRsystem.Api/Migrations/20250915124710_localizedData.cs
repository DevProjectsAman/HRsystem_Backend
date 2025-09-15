using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class localizedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 15, 15, 47, 6, 179, DateTimeKind.Local).AddTicks(1471), new DateTime(2025, 9, 15, 12, 47, 6, 179, DateTimeKind.Utc).AddTicks(4533), new Guid("6f2c79a9-2e62-47da-927c-5fdb4484fb1a") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 14, 15, 17, 41, 63, DateTimeKind.Local).AddTicks(3039), new DateTime(2025, 9, 14, 12, 17, 41, 63, DateTimeKind.Utc).AddTicks(7542), new Guid("893561e0-7a04-4700-bd45-55eb5843fc80") });
        }
    }
}
