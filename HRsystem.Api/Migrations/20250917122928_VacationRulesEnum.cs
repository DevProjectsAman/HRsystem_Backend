using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class VacationRulesEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Tb_Vacation_Rule",
                type: "ENUM('Male','Female','All')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Male','Female')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 29, 27, 968, DateTimeKind.Local).AddTicks(8444), new DateTime(2025, 9, 17, 12, 29, 27, 969, DateTimeKind.Utc).AddTicks(1503), new Guid("6afe1ece-d32a-4a93-94c7-f87bd5d4e46b") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Tb_Vacation_Rule",
                type: "ENUM('Male','Female')",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ENUM('Male','Female','All')")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 17, 15, 21, 20, 120, DateTimeKind.Local).AddTicks(8882), new DateTime(2025, 9, 17, 12, 21, 20, 121, DateTimeKind.Utc).AddTicks(2309), new Guid("f47cd4d3-3720-41e7-b9cb-652e6f3d86c6") });
        }
    }
}
