using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class shiftrules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityID",
                table: "Tb_Shift_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GovID",
                table: "Tb_Shift_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChangedBy",
                table: "Tb_Employee_Activity_Approval",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 22, 11, 46, 38, 494, DateTimeKind.Local).AddTicks(9327), new DateTime(2025, 9, 22, 8, 46, 38, 495, DateTimeKind.Utc).AddTicks(2409), new Guid("e02165df-41a9-4cfb-b996-1c92adbea07b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityID",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropColumn(
                name: "GovID",
                table: "Tb_Shift_Rule");

            migrationBuilder.AlterColumn<int>(
                name: "ChangedBy",
                table: "Tb_Employee_Activity_Approval",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 21, 13, 1, 59, 473, DateTimeKind.Local).AddTicks(3414), new DateTime(2025, 9, 21, 10, 1, 59, 473, DateTimeKind.Utc).AddTicks(7699), new Guid("3c477e0d-da3c-4808-9234-eb88ee44381e") });
        }
    }
}
