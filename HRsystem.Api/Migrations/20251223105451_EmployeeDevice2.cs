using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeDevice2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tb_Employee_Devices",
                newName: "DeviceUid");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetByUserDate",
                table: "Tb_Employee_Devices",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResetByUserId",
                table: "Tb_Employee_Devices",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 23, 10, 54, 47, 739, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 12, 23, 10, 54, 47, 740, DateTimeKind.Utc).AddTicks(2547), new Guid("43769844-8f9c-431f-9127-ac0cc5de830b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetByUserDate",
                table: "Tb_Employee_Devices");

            migrationBuilder.DropColumn(
                name: "ResetByUserId",
                table: "Tb_Employee_Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceUid",
                table: "Tb_Employee_Devices",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 22, 11, 50, 34, 201, DateTimeKind.Utc).AddTicks(6654), new DateTime(2025, 12, 22, 11, 50, 34, 201, DateTimeKind.Utc).AddTicks(9892), new Guid("2ddaaf47-dc78-48be-af60-096173cd5be3") });
        }
    }
}
