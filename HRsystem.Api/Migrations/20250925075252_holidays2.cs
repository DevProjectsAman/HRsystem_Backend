using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class holidays2 : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "RemoteWorkDaysNames",
                table: "Tb_Remote_WorkDays",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_holiday_types",
                columns: table => new
                {
                    HolidayTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HolidayTypeName = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_holiday_types", x => x.HolidayTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_holidays",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HolidayTypeId = table.Column<int>(type: "int", nullable: false),
                    HolidayName = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsForChristiansOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_holidays", x => x.HolidayId);
                    table.ForeignKey(
                        name: "FK_tb_holidays_tb_holiday_types_HolidayTypeId",
                        column: x => x.HolidayTypeId,
                        principalTable: "tb_holiday_types",
                        principalColumn: "HolidayTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 25, 10, 52, 51, 275, DateTimeKind.Local).AddTicks(8586), new DateTime(2025, 9, 25, 7, 52, 51, 276, DateTimeKind.Utc).AddTicks(1778), new Guid("aa364ee7-d004-4d70-87f5-9280549109b3") });

            migrationBuilder.CreateIndex(
                name: "IX_tb_holidays_HolidayTypeId",
                table: "tb_holidays",
                column: "HolidayTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_holidays");

            migrationBuilder.DropTable(
                name: "tb_holiday_types");

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
                table: "Tb_Remote_WorkDays",
                keyColumn: "RemoteWorkDaysNames",
                keyValue: null,
                column: "RemoteWorkDaysNames",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RemoteWorkDaysNames",
                table: "Tb_Remote_WorkDays",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 15, 30, 56, 860, DateTimeKind.Local).AddTicks(9685), new DateTime(2025, 9, 24, 12, 30, 56, 861, DateTimeKind.Utc).AddTicks(2809), new Guid("746d1e45-5801-46d2-81ae-4ae69e745b43") });
        }
    }
}
