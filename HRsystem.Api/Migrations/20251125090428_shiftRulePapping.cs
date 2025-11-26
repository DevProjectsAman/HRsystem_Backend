using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class shiftRulePapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Shift_Rule_Mapping",
                columns: table => new
                {
                    RuleMappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShiftRuleId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Shift_Rule_Mapping", x => x.RuleMappingId);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Mapping_Tb_Shift_Rule_ShiftRuleId",
                        column: x => x.ShiftRuleId,
                        principalTable: "Tb_Shift_Rule",
                        principalColumn: "RuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Shift_Rule_Mapping_Tb_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Tb_Shift",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 25, 11, 4, 27, 152, DateTimeKind.Local).AddTicks(3693), new DateTime(2025, 11, 25, 11, 4, 27, 152, DateTimeKind.Local).AddTicks(6789), new Guid("b2ea1d6b-9ce7-4420-b998-a89d695f7737") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_Mapping_ShiftId",
                table: "Tb_Shift_Rule_Mapping",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Shift_Rule_Mapping_ShiftRuleId",
                table: "Tb_Shift_Rule_Mapping",
                column: "ShiftRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Shift_Rule_Mapping");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 25, 8, 50, 48, 321, DateTimeKind.Local).AddTicks(9777), new DateTime(2025, 11, 25, 8, 50, 48, 322, DateTimeKind.Local).AddTicks(3187), new Guid("972dc7c8-ec9b-47d0-9367-68e0ce5e6754") });
        }
    }
}
