using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class shiftrulesJobLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

             
  
            

            migrationBuilder.AddColumn<int>(
                name: "JobLevelId",
                table: "Tb_Shift_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 9, 31, 59, 745, DateTimeKind.Local).AddTicks(3374), new DateTime(2025, 11, 9, 7, 31, 59, 745, DateTimeKind.Utc).AddTicks(6606), new Guid("f72fd56a-71e0-41cc-9b37-7d52d5afd618") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_JobLevelId",
                table: "Tb_Shift_Rule",
                column: "JobLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Job_Level_JobLevelId",
                table: "Tb_Shift_Rule",
                column: "JobLevelId",
                principalTable: "Tb_Job_Level",
                principalColumn: "JobLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Job_Level_JobLevelId",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Shift_Rule_JobLevelId",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropColumn(
                name: "JobLevelId",
                table: "Tb_Shift_Rule");

            

            

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 6, 15, 44, 20, 781, DateTimeKind.Local).AddTicks(8953), new DateTime(2025, 11, 6, 13, 44, 20, 782, DateTimeKind.Utc).AddTicks(3127), new Guid("243c0749-5a46-48f1-b49b-365139764c67") });

            
            

            

            
        }
    }
}
