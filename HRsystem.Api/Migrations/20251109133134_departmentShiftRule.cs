using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class departmentShiftRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Tb_Shift_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentsDepartmentId",
                table: "Tb_Shift_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 15, 31, 31, 287, DateTimeKind.Local).AddTicks(3613), new DateTime(2025, 11, 9, 13, 31, 31, 287, DateTimeKind.Utc).AddTicks(6895), new Guid("306e9e5a-11f9-45ac-970e-f4fcb53ef302") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_DepartmentsDepartmentId",
                table: "Tb_Shift_Rule",
                column: "DepartmentsDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Department_DepartmentsDepartmentId",
                table: "Tb_Shift_Rule",
                column: "DepartmentsDepartmentId",
                principalTable: "Tb_Department",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Department_DepartmentsDepartmentId",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Shift_Rule_DepartmentsDepartmentId",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropColumn(
                name: "DepartmentsDepartmentId",
                table: "Tb_Shift_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 13, 15, 57, 562, DateTimeKind.Local).AddTicks(9329), new DateTime(2025, 11, 9, 11, 15, 57, 563, DateTimeKind.Utc).AddTicks(3284), new Guid("cae5bacd-2e75-4da5-985b-082626b2d1de") });
        }
    }
}
