using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class TbUserSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_UserSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClientType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Jti = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_UserSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 5, 11, 47, 47, 704, DateTimeKind.Utc).AddTicks(2437), new DateTime(2026, 1, 5, 11, 47, 47, 704, DateTimeKind.Utc).AddTicks(5601), new Guid("14990f74-adff-4234-ac2c-5038cecf2889") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_UserSessions_UserId",
                table: "Tb_UserSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_UserSessions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 4, 11, 16, 18, 558, DateTimeKind.Utc).AddTicks(6613), new DateTime(2026, 1, 4, 11, 16, 18, 559, DateTimeKind.Utc).AddTicks(1176), new Guid("36d98634-67f5-42bb-b53f-d6941d042354") });
        }
    }
}
