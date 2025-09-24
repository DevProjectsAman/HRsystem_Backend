using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateworkdays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkDaysNames",
                table: "Tb_WorkDays",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 15, 22, 54, 867, DateTimeKind.Local).AddTicks(1229), new DateTime(2025, 9, 24, 12, 22, 54, 867, DateTimeKind.Utc).AddTicks(4785), new Guid("f605c6d3-6133-48aa-be20-ca61bfef7998") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkDaysNames",
                table: "Tb_WorkDays",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 44, 49, 776, DateTimeKind.Local).AddTicks(8554), new DateTime(2025, 9, 24, 11, 44, 49, 777, DateTimeKind.Utc).AddTicks(5194), new Guid("5b7c73fc-0888-43b6-a443-df8714365eb4") });
        }
    }
}
