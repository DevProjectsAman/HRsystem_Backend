using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class vacationrulepriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Tb_Vacation_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 10, 1, 16, 43, 14, 506, DateTimeKind.Local).AddTicks(9165), new DateTime(2025, 10, 1, 13, 43, 14, 507, DateTimeKind.Utc).AddTicks(2321), new Guid("9dcc46c7-c0a8-425f-8480-fd49a48cf541") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tb_Vacation_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 46, 39, 881, DateTimeKind.Local).AddTicks(6188), new DateTime(2025, 9, 28, 10, 46, 39, 881, DateTimeKind.Utc).AddTicks(9580), new Guid("9ef4f009-053c-4f26-92ef-057fcc88e0ea") });
        }
    }
}
