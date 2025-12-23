using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeDevice4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Tb_Employee_Devices");

            //migrationBuilder.RenameColumn(
            //    name: "DeviceUid",
            //    table: "Tb_Employee_Attendance_Punch",
            //    newName: "DeviceId");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Devices_Track",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DeviceUid = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    OsVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Manufacturer = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPhysicalDevice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeviceFingerprint = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AppVersion = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisteredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActiveDevice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ResetByUserId = table.Column<int>(type: "int", nullable: true),
                    ResetByUserDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Devices_Track", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 23, 12, 3, 2, 998, DateTimeKind.Utc).AddTicks(6111), new DateTime(2025, 12, 23, 12, 3, 2, 998, DateTimeKind.Utc).AddTicks(9332), new Guid("865e007d-8459-410a-95c4-bcb7092feb76") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Employee_Devices_Track");

            //migrationBuilder.RenameColumn(
            //    name: "DeviceId",
            //    table: "Tb_Employee_Attendance_Punch",
            //    newName: "DeviceUid");

            migrationBuilder.CreateTable(
                name: "Tb_Employee_Devices",
                columns: table => new
                {
                    DeviceUid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeviceUuid = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsActiveDevice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPhysical = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastActiveAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NativeOsId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OsType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OsVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegisteredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResetByUserDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResetByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employee_Devices", x => x.DeviceUid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 12, 23, 10, 54, 47, 739, DateTimeKind.Utc).AddTicks(9256), new DateTime(2025, 12, 23, 10, 54, 47, 740, DateTimeKind.Utc).AddTicks(2547), new Guid("43769844-8f9c-431f-9127-ac0cc5de830b") });
        }
    }
}
