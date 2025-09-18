using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class vacationtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeductable",
                table: "Tb_Vacation_Type",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 18, 15, 16, 30, 806, DateTimeKind.Local).AddTicks(3573), new DateTime(2025, 9, 18, 12, 16, 30, 806, DateTimeKind.Utc).AddTicks(6759), new Guid("30f745a5-96d4-407b-a9c3-14883ccb79a5") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeductable",
                table: "Tb_Vacation_Type");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 18, 13, 20, 55, 831, DateTimeKind.Local).AddTicks(1525), new DateTime(2025, 9, 18, 10, 20, 55, 831, DateTimeKind.Utc).AddTicks(4738), new Guid("c73408f5-a11b-426f-b3a5-0963c3089adc") });
        }
    }
}
