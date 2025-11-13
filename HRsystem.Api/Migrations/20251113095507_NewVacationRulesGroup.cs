using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewVacationRulesGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Vacation_Rules_Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinAge = table.Column<int>(type: "int", nullable: true),
                    MaxAge = table.Column<int>(type: "int", nullable: true),
                    MinServiceYears = table.Column<int>(type: "int", nullable: true),
                    MaxServiceYears = table.Column<int>(type: "int", nullable: true),
                    WorkingYearsAtCompany = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Vacation_Rules_Group", x => x.GroupId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tb_Vacation_Rules_Group_Detail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    VacationTypeId = table.Column<int>(type: "int", nullable: false),
                    YearlyBalance = table.Column<int>(type: "int", nullable: false),
                    Prorate = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Religion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Vacation_Rules_Group_Detail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_Tb_Vacation_Rules_Group_Detail_Tb_Vacation_Rules_Group_Group~",
                        column: x => x.GroupId,
                        principalTable: "Tb_Vacation_Rules_Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Vacation_Rules_Group_Detail_Tb_Vacation_Type_VacationType~",
                        column: x => x.VacationTypeId,
                        principalTable: "Tb_Vacation_Type",
                        principalColumn: "VacationTypeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 13, 11, 55, 2, 193, DateTimeKind.Local).AddTicks(830), new DateTime(2025, 11, 13, 11, 55, 2, 193, DateTimeKind.Local).AddTicks(3999), new Guid("7ebf725f-71f8-4483-b700-d84c13dcce9f") });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Vacation_Rules_Group_Detail_GroupId",
                table: "Tb_Vacation_Rules_Group_Detail",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Vacation_Rules_Group_Detail_VacationTypeId",
                table: "Tb_Vacation_Rules_Group_Detail",
                column: "VacationTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Vacation_Rules_Group_Detail");

            migrationBuilder.DropTable(
                name: "Tb_Vacation_Rules_Group");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 12, 13, 3, 42, 453, DateTimeKind.Local).AddTicks(6988), new DateTime(2025, 11, 12, 13, 3, 42, 454, DateTimeKind.Local).AddTicks(343), new Guid("2cef21c8-3746-4cc5-8d82-f1e6bce26172") });
        }
    }
}
