using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class employeeshiftdepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 16, 11, 47, 23, 181, DateTimeKind.Local).AddTicks(133), new DateTime(2025, 9, 16, 8, 47, 23, 181, DateTimeKind.Utc).AddTicks(2932), new Guid("e0f15d25-34a9-4912-bf58-f83855a2585d") });

            

           

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Department_DepartmentId",
                table: "Tb_Employee",
                column: "DepartmentId",
                principalTable: "Tb_Department",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee",
                column: "JobTitleId",
                principalTable: "Tb_Job_Title",
                principalColumn: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Shift_ShiftId",
                table: "Tb_Employee",
                column: "ShiftId",
                principalTable: "Tb_Shift",
                principalColumn: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Department_DepartmentId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Shift_ShiftId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_DepartmentId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_ShiftId",
                table: "Tb_Employee");

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleId",
                table: "Tb_Employee",
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
                values: new object[] { new DateTime(2025, 9, 15, 15, 47, 6, 179, DateTimeKind.Local).AddTicks(1471), new DateTime(2025, 9, 15, 12, 47, 6, 179, DateTimeKind.Utc).AddTicks(4533), new Guid("6f2c79a9-2e62-47da-927c-5fdb4484fb1a") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee",
                column: "JobTitleId",
                principalTable: "Tb_Job_Title",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
