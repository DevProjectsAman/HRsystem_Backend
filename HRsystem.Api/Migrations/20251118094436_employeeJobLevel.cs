using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class employeeJobLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobLevelId",
                table: "Tb_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 18, 11, 44, 31, 875, DateTimeKind.Local).AddTicks(2956), new DateTime(2025, 11, 18, 11, 44, 31, 875, DateTimeKind.Local).AddTicks(6034), new Guid("32175154-2a0f-45cd-a0d5-46690e8cfc44") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_JobLevelId",
                table: "Tb_Employee",
                column: "JobLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Level_JobLevelId",
                table: "Tb_Employee",
                column: "JobLevelId",
                principalTable: "Tb_Job_Level",
                principalColumn: "JobLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Level_JobLevelId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_JobLevelId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "JobLevelId",
                table: "Tb_Employee");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 13, 11, 55, 2, 193, DateTimeKind.Local).AddTicks(830), new DateTime(2025, 11, 13, 11, 55, 2, 193, DateTimeKind.Local).AddTicks(3999), new Guid("7ebf725f-71f8-4483-b700-d84c13dcce9f") });
        }
    }
}
