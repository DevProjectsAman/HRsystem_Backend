using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class vacationRule_company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Tb_Vacation_Rule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 13, 3, 42, 453, DateTimeKind.Local).AddTicks(6988), new DateTime(2025, 11, 12, 13, 3, 42, 454, DateTimeKind.Local).AddTicks(343), new Guid("2cef21c8-3746-4cc5-8d82-f1e6bce26172") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tb_Vacation_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 10, 5, 44, 635, DateTimeKind.Local).AddTicks(4182), new DateTime(2025, 11, 12, 10, 5, 44, 635, DateTimeKind.Local).AddTicks(8403), new Guid("23a1807c-d886-4d08-9a88-927c3b0cd04a") });
        }
    }
}
