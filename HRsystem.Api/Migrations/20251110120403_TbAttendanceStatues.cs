using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class TbAttendanceStatues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Attendance_Statues",
                columns: table => new
                {
                    AttendanceStatuesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendanceStatuesCode = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttendanceStatuesName = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Attendance_Statues", x => x.AttendanceStatuesId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 10, 14, 3, 52, 481, DateTimeKind.Local).AddTicks(5457), new DateTime(2025, 11, 10, 12, 3, 52, 482, DateTimeKind.Utc).AddTicks(1178), new Guid("30c862d6-8660-4f5b-b404-3384ee4d9f0c") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Attendance_Statues");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 16, 9, 29, 894, DateTimeKind.Local).AddTicks(1455), new DateTime(2025, 11, 9, 14, 9, 29, 894, DateTimeKind.Utc).AddTicks(4467), new Guid("59c7808a-7ee2-4880-81c1-4e2a8e3685d7") });
        }
    }
}
