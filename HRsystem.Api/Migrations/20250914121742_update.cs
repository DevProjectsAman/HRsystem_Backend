using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Tb_Project",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Tb_Department",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 14, 15, 17, 41, 63, DateTimeKind.Local).AddTicks(3039), new DateTime(2025, 9, 14, 12, 17, 41, 63, DateTimeKind.Utc).AddTicks(7542), new Guid("893561e0-7a04-4700-bd45-55eb5843fc80") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Tb_Project",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                table: "Tb_Department",
                type: "varchar(55)",
                maxLength: 55,
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
                values: new object[] { new DateTime(2025, 9, 14, 14, 1, 30, 980, DateTimeKind.Local).AddTicks(9765), new DateTime(2025, 9, 14, 11, 1, 30, 981, DateTimeKind.Utc).AddTicks(2971), new Guid("80b0e0e5-5203-4d51-b716-793c11f15a70") });
        }
    }
}
