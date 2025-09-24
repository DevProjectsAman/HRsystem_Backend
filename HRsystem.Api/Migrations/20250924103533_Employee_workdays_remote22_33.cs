using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class Employee_workdays_remote22_33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Remote_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropIndex(
                name: "IX_Tb_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Remote_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays");

            migrationBuilder.DropColumn(
                name: "TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropColumn(
                name: "TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 23, 39, 242, DateTimeKind.Local).AddTicks(3767), new DateTime(2025, 9, 24, 10, 23, 39, 242, DateTimeKind.Utc).AddTicks(6935), new Guid("3fac8cb5-586c-4626-8156-aec31a469a5e") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                column: "TbEmployeeEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Remote_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays",
                column: "TbEmployeeEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Remote_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays",
                column: "TbEmployeeEmployeeId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                column: "TbEmployeeEmployeeId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");
        }
    }
}