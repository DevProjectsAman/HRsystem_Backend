using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class employee_days : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Remote_WorkDays_TbRemoteWorkDaysRemoteWorkDay~",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_TbWorkDaysWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_TbRemoteWorkDaysRemoteWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_TbWorkDaysWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "TbRemoteWorkDaysRemoteWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "TbWorkDaysWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.AddColumn<int>(
                name: "JobTitleId",
                table: "Tb_Work_Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Tb_Work_Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkingLocationWorkLocationId",
                table: "Tb_Work_Location",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 15, 30, 56, 860, DateTimeKind.Local).AddTicks(9685), new DateTime(2025, 9, 24, 12, 30, 56, 861, DateTimeKind.Utc).AddTicks(2809), new Guid("746d1e45-5801-46d2-81ae-4ae69e745b43") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Work_Location_JobTitleId",
                table: "Tb_Work_Location",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Work_Location_ProjectId",
                table: "Tb_Work_Location",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Work_Location_WorkingLocationWorkLocationId",
                table: "Tb_Work_Location",
                column: "WorkingLocationWorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_RemoteWorkDaysId",
                table: "Tb_Employee",
                column: "RemoteWorkDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_WorkDaysId",
                table: "Tb_Employee",
                column: "WorkDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Remote_WorkDays_RemoteWorkDaysId",
                table: "Tb_Employee",
                column: "RemoteWorkDaysId",
                principalTable: "Tb_Remote_WorkDays",
                principalColumn: "RemoteWorkDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
                table: "Tb_Employee",
                column: "WorkDaysId",
                principalTable: "Tb_WorkDays",
                principalColumn: "WorkDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Work_Location_Tb_Job_Title_JobTitleId",
                table: "Tb_Work_Location",
                column: "JobTitleId",
                principalTable: "Tb_Job_Title",
                principalColumn: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Work_Location_Tb_Project_ProjectId",
                table: "Tb_Work_Location",
                column: "ProjectId",
                principalTable: "Tb_Project",
                principalColumn: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Work_Location_Tb_Work_Location_WorkingLocationWorkLocatio~",
                table: "Tb_Work_Location",
                column: "WorkingLocationWorkLocationId",
                principalTable: "Tb_Work_Location",
                principalColumn: "WorkLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Remote_WorkDays_RemoteWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Work_Location_Tb_Job_Title_JobTitleId",
                table: "Tb_Work_Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Work_Location_Tb_Project_ProjectId",
                table: "Tb_Work_Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Work_Location_Tb_Work_Location_WorkingLocationWorkLocatio~",
                table: "Tb_Work_Location");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Work_Location_JobTitleId",
                table: "Tb_Work_Location");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Work_Location_ProjectId",
                table: "Tb_Work_Location");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Work_Location_WorkingLocationWorkLocationId",
                table: "Tb_Work_Location");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_RemoteWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Employee_WorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "Tb_Work_Location");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tb_Work_Location");

            migrationBuilder.DropColumn(
                name: "WorkingLocationWorkLocationId",
                table: "Tb_Work_Location");

            migrationBuilder.AddColumn<int>(
                name: "TbRemoteWorkDaysRemoteWorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TbWorkDaysWorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 35, 33, 75, DateTimeKind.Local).AddTicks(4629), new DateTime(2025, 9, 24, 10, 35, 33, 75, DateTimeKind.Utc).AddTicks(7673), new Guid("72cb14f5-d9b1-4422-9d74-431bd9d7cb55") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_TbRemoteWorkDaysRemoteWorkDaysId",
                table: "Tb_Employee",
                column: "TbRemoteWorkDaysRemoteWorkDaysId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Employee_TbWorkDaysWorkDaysId",
                table: "Tb_Employee",
                column: "TbWorkDaysWorkDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Remote_WorkDays_TbRemoteWorkDaysRemoteWorkDay~",
                table: "Tb_Employee",
                column: "TbRemoteWorkDaysRemoteWorkDaysId",
                principalTable: "Tb_Remote_WorkDays",
                principalColumn: "RemoteWorkDaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_TbWorkDaysWorkDaysId",
                table: "Tb_Employee",
                column: "TbWorkDaysWorkDaysId",
                principalTable: "Tb_WorkDays",
                principalColumn: "WorkDaysId");
        }
    }
}
