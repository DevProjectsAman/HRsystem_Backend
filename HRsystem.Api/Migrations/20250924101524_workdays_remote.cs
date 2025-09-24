using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class workdays_remote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkDaysDescriptions",
                table: "Tb_WorkDays",
                newName: "WorkDaysNames");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 15, 23, 843, DateTimeKind.Local).AddTicks(7), new DateTime(2025, 9, 24, 10, 15, 23, 843, DateTimeKind.Utc).AddTicks(3056), new Guid("62be847a-939c-46ce-9995-227c085bb3d4") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkDaysNames",
                table: "Tb_WorkDays",
                newName: "WorkDaysDescriptions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 9, 56, 218, DateTimeKind.Local).AddTicks(4295), new DateTime(2025, 9, 24, 10, 9, 56, 218, DateTimeKind.Utc).AddTicks(7502), new Guid("c9a43bb9-10e7-41ce-ac9c-1e6160b0e95e") });
        }
    }
}
