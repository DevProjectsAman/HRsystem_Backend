using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class workdaysRulesChanges22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropIndex(
                name: "IX_Tb_WorkDays_Rules_ShiftId",
                table: "Tb_WorkDays_Rules");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Tb_WorkDays_Rules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 11, 20, 5, 400, DateTimeKind.Local).AddTicks(2831), new DateTime(2025, 11, 11, 11, 20, 5, 400, DateTimeKind.Local).AddTicks(5794), new Guid("d5b5e882-fcf2-499e-881b-9d74c04128da") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Tb_WorkDays_Rules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 11, 8, 40, 256, DateTimeKind.Local).AddTicks(2558), new DateTime(2025, 11, 11, 11, 8, 40, 256, DateTimeKind.Local).AddTicks(5691), new Guid("02cbdbd0-9168-4920-b263-57634ab20104") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_WorkDays_Rules_ShiftId",
                table: "Tb_WorkDays_Rules",
                column: "ShiftId");

             
        }
    }
}
