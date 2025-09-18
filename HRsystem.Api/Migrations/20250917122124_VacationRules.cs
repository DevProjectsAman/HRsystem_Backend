using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class VacationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingYearsAtCompany",
                table: "Tb_Vacation_Rule",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 21, 20, 120, DateTimeKind.Local).AddTicks(8882), new DateTime(2025, 9, 17, 12, 21, 20, 121, DateTimeKind.Utc).AddTicks(2309), new Guid("f47cd4d3-3720-41e7-b9cb-652e6f3d86c6") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingYearsAtCompany",
                table: "Tb_Vacation_Rule");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 14, 49, 4, 256, DateTimeKind.Local).AddTicks(9388), new DateTime(2025, 9, 17, 11, 49, 4, 257, DateTimeKind.Utc).AddTicks(7138), new Guid("c4baeba0-6544-44ad-9b90-746d40c2348e") });
        }
    }
}
