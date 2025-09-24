using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitRemoteWorkDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Remote_Work_Days",
                columns: table => new
                {
                    RemoteWorkDaysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RemoteWorkDaysNames = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Remote_Work_Days", x => x.RemoteWorkDaysId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 14, 44, 49, 776, DateTimeKind.Local).AddTicks(8554), new DateTime(2025, 9, 24, 11, 44, 49, 777, DateTimeKind.Utc).AddTicks(5194), new Guid("5b7c73fc-0888-43b6-a443-df8714365eb4") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Remote_Work_Days");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 24, 13, 15, 23, 843, DateTimeKind.Local).AddTicks(7), new DateTime(2025, 9, 24, 10, 15, 23, 843, DateTimeKind.Utc).AddTicks(3056), new Guid("62be847a-939c-46ce-9995-227c085bb3d4") });
        }
    }
}
