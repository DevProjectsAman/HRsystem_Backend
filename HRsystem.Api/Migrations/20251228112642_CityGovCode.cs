using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class CityGovCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoveCode",
                table: "Tb_City",
                type: "varchar(15)",
                maxLength: 15,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 28, 11, 26, 41, 902, DateTimeKind.Utc).AddTicks(8421), new DateTime(2025, 12, 28, 11, 26, 41, 903, DateTimeKind.Utc).AddTicks(1525), new Guid("ebd8c46e-1af0-41e1-9a86-b930e38075a9") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoveCode",
                table: "Tb_City");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 25, 8, 0, 30, 38, DateTimeKind.Utc).AddTicks(3307), new DateTime(2025, 12, 25, 8, 0, 30, 38, DateTimeKind.Utc).AddTicks(6429), new Guid("ede85ed7-ad3e-42f0-a524-c51d0154ade1") });
        }
    }
}
