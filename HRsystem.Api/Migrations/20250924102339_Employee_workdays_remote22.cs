using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class Employee_workdays_remote22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Remote_Work_Days_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_Work_Days");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Remote_Work_Days",
                table: "Tb_Remote_Work_Days");

            migrationBuilder.RenameTable(
                name: "Tb_Remote_Work_Days",
                newName: "Tb_Remote_WorkDays");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_Remote_Work_Days_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays",
                newName: "IX_Tb_Remote_WorkDays_TbEmployeeEmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Remote_WorkDays",
                table: "Tb_Remote_WorkDays",
                column: "RemoteWorkDaysId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 23, 39, 242, DateTimeKind.Local).AddTicks(3767), new DateTime(2025, 9, 24, 10, 23, 39, 242, DateTimeKind.Utc).AddTicks(6935), new Guid("3fac8cb5-586c-4626-8156-aec31a469a5e") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Remote_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays",
                column: "TbEmployeeEmployeeId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Remote_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_WorkDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Remote_WorkDays",
                table: "Tb_Remote_WorkDays");

            migrationBuilder.RenameTable(
                name: "Tb_Remote_WorkDays",
                newName: "Tb_Remote_Work_Days");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_Remote_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_Remote_Work_Days",
                newName: "IX_Tb_Remote_Work_Days_TbEmployeeEmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Remote_Work_Days",
                table: "Tb_Remote_Work_Days",
                column: "RemoteWorkDaysId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 22, 27, 273, DateTimeKind.Local).AddTicks(3480), new DateTime(2025, 9, 24, 10, 22, 27, 273, DateTimeKind.Utc).AddTicks(6666), new Guid("fd12433f-e49f-45b8-96f9-7d96581aa915") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Remote_Work_Days_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_Remote_Work_Days",
                column: "TbEmployeeEmployeeId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");
        }
    }
}