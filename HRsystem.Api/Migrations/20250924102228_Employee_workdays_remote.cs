using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class Employee_workdays_remote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemoteWorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tb_Remote_Work_Days",
                columns: table => new
                {
                    RemoteWorkDaysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RemoteWorkDaysNames = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TbEmployeeEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Remote_Work_Days", x => x.RemoteWorkDaysId);
                    table.ForeignKey(
                        name: "FK_Tb_Remote_Work_Days_Tb_Employee_TbEmployeeEmployeeId",
                        column: x => x.TbEmployeeEmployeeId,
                        principalTable: "Tb_Employee",
                        principalColumn: "EmployeeId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 22, 27, 273, DateTimeKind.Local).AddTicks(3480), new DateTime(2025, 9, 24, 10, 22, 27, 273, DateTimeKind.Utc).AddTicks(6666), new Guid("fd12433f-e49f-45b8-96f9-7d96581aa915") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                column: "TbEmployeeEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Remote_Work_Days_TbEmployeeEmployeeId",
                table: "Tb_Remote_Work_Days",
                column: "TbEmployeeEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_WorkDays",
                column: "TbEmployeeEmployeeId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_WorkDays_Tb_Employee_TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropTable(
                name: "Tb_Remote_Work_Days");

            migrationBuilder.DropIndex(
                name: "IX_Tb_WorkDays_TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropColumn(
                name: "TbEmployeeEmployeeId",
                table: "Tb_WorkDays");

            migrationBuilder.DropColumn(
                name: "RemoteWorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "WorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 15, 23, 843, DateTimeKind.Local).AddTicks(7), new DateTime(2025, 9, 24, 10, 15, 23, 843, DateTimeKind.Utc).AddTicks(3056), new Guid("62be847a-939c-46ce-9995-227c085bb3d4") });
        }
    }
}