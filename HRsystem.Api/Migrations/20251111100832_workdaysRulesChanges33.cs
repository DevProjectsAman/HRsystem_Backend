using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class workdaysRulesChanges33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 12, 8, 29, 592, DateTimeKind.Local).AddTicks(8054), new DateTime(2025, 11, 11, 12, 8, 29, 593, DateTimeKind.Local).AddTicks(1482), new Guid("74e5ca98-de9f-47e8-bd48-432710640da3") });

            

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_WorkDays_Rules_Tb_WorkDays_WorkDaysId",
                table: "Tb_WorkDays_Rules",
                column: "WorkDaysId",
                principalTable: "Tb_WorkDays",
                principalColumn: "WorkDaysId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_WorkDays_Rules_Tb_WorkDays_WorkDaysId",
                table: "Tb_WorkDays_Rules");

            

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 11, 20, 5, 400, DateTimeKind.Local).AddTicks(2831), new DateTime(2025, 11, 11, 11, 20, 5, 400, DateTimeKind.Local).AddTicks(5794), new Guid("d5b5e882-fcf2-499e-881b-9d74c04128da") });
        }
    }
}
