using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class ShiftRulesNewIncludes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 13, 15, 57, 562, DateTimeKind.Local).AddTicks(9329), new DateTime(2025, 11, 9, 11, 15, 57, 563, DateTimeKind.Utc).AddTicks(3284), new Guid("cae5bacd-2e75-4da5-985b-082626b2d1de") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_CityID",
                table: "Tb_Shift_Rule",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_GovID",
                table: "Tb_Shift_Rule",
                column: "GovID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_City_CityID",
                table: "Tb_Shift_Rule",
                column: "CityID",
                principalTable: "Tb_City",
                principalColumn: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Gov_GovID",
                table: "Tb_Shift_Rule",
                column: "GovID",
                principalTable: "Tb_Gov",
                principalColumn: "GovId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_City_CityID",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Shift_Rule_Tb_Gov_GovID",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Shift_Rule_CityID",
                table: "Tb_Shift_Rule");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Shift_Rule_GovID",
                table: "Tb_Shift_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 9, 31, 59, 745, DateTimeKind.Local).AddTicks(3374), new DateTime(2025, 11, 9, 7, 31, 59, 745, DateTimeKind.Utc).AddTicks(6606), new Guid("f72fd56a-71e0-41cc-9b37-7d52d5afd618") });
        }
    }
}
