using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class workdays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_WorkDays",
                columns: table => new
                {
                    WorkDaysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkDaysDescription = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkDaysDescriptions = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_WorkDays", x => x.WorkDaysId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_WorkDays_Rules",
                columns: table => new
                {
                    WorkDaysRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GovID = table.Column<int>(type: "int", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    JobTitleId = table.Column<int>(type: "int", nullable: true),
                    WorkingLocationId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    WorkDaysId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_WorkDays_Rules", x => x.WorkDaysRuleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 9, 56, 218, DateTimeKind.Local).AddTicks(4295), new DateTime(2025, 9, 24, 10, 9, 56, 218, DateTimeKind.Utc).AddTicks(7502), new Guid("c9a43bb9-10e7-41ce-ac9c-1e6160b0e95e") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_WorkDays");

            migrationBuilder.DropTable(
                name: "Tb_WorkDays_Rules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 22, 11, 46, 38, 494, DateTimeKind.Local).AddTicks(9327), new DateTime(2025, 9, 22, 8, 46, 38, 495, DateTimeKind.Utc).AddTicks(2409), new Guid("e02165df-41a9-4cfb-b996-1c92adbea07b") });
        }
    }
}
