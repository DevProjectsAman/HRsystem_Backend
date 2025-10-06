using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class TbActivityStatusWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 1, 9, 54, 45, 662, DateTimeKind.Local).AddTicks(7624), new DateTime(2025, 10, 1, 6, 54, 45, 663, DateTimeKind.Utc).AddTicks(2213), new Guid("37945f5e-5a16-4328-aa17-9b99885194a9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 46, 39, 881, DateTimeKind.Local).AddTicks(6188), new DateTime(2025, 9, 28, 10, 46, 39, 881, DateTimeKind.Utc).AddTicks(9580), new Guid("9ef4f009-053c-4f26-92ef-057fcc88e0ea") });
        }
    }
}
