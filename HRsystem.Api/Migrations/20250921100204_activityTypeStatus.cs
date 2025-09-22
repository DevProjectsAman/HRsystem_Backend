using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class activityTypeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_activity_type_status",
                columns: table => new
                {
                    ActivityTypeStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityTypeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_activity_type_status", x => x.ActivityTypeStatusId);
                    table.ForeignKey(
                        name: "FK_tb_activity_type_status_Tb_Activity_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Tb_Activity_Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_activity_type_status_Tb_Activity_Type_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "Tb_Activity_Type",
                        principalColumn: "ActivityTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_activity_type_status_Tb_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tb_Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 21, 13, 1, 59, 473, DateTimeKind.Local).AddTicks(3414), new DateTime(2025, 9, 21, 10, 1, 59, 473, DateTimeKind.Utc).AddTicks(7699), new Guid("3c477e0d-da3c-4808-9234-eb88ee44381e") });

            migrationBuilder.CreateIndex(
                name: "IX_tb_activity_type_status_ActivityTypeId",
                table: "tb_activity_type_status",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_activity_type_status_CompanyId",
                table: "tb_activity_type_status",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_activity_type_status_StatusId",
                table: "tb_activity_type_status",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_activity_type_status");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 18, 15, 16, 30, 806, DateTimeKind.Local).AddTicks(3573), new DateTime(2025, 9, 18, 12, 16, 30, 806, DateTimeKind.Utc).AddTicks(6759), new Guid("30f745a5-96d4-407b-a9c3-14883ccb79a5") });
        }
    }
}
