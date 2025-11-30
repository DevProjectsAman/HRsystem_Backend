using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class LatLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Tb_Work_Location",
                type: "decimal(11,8)",
                precision: 11,
                scale: 8,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Tb_Work_Location",
                type: "decimal(11,8)",
                precision: 11,
                scale: 8,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 30, 11, 11, 16, 197, DateTimeKind.Local).AddTicks(8327), new DateTime(2025, 11, 30, 11, 11, 16, 198, DateTimeKind.Local).AddTicks(1431), new Guid("ce00fde0-6ed9-48b5-aa57-c2b06d64a4ff") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Tb_Work_Location",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,8)",
                oldPrecision: 11,
                oldScale: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Tb_Work_Location",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,8)",
                oldPrecision: 11,
                oldScale: 8,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 25, 11, 4, 27, 152, DateTimeKind.Local).AddTicks(3693), new DateTime(2025, 11, 25, 11, 4, 27, 152, DateTimeKind.Local).AddTicks(6789), new Guid("b2ea1d6b-9ce7-4420-b998-a89d695f7737") });
        }
    }
}
