using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class fcmToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "Tb_User_Login_History",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Tb_User_Login_History",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserNameAttempt",
                table: "Tb_User_Login_History",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Tb_Employee_Devices_Track",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 2, 22, 8, 6, 12, 443, DateTimeKind.Utc).AddTicks(9351), new DateTime(2026, 2, 22, 8, 6, 12, 444, DateTimeKind.Utc).AddTicks(2496), new Guid("e5763a20-bd0a-4596-94bd-cf5733688ce2") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Tb_User_Login_History");

            migrationBuilder.DropColumn(
                name: "UserNameAttempt",
                table: "Tb_User_Login_History");

            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Tb_Employee_Devices_Track");

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "Tb_User_Login_History",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 25, 9, 16, 17, 107, DateTimeKind.Utc).AddTicks(4186), new DateTime(2026, 1, 25, 9, 16, 17, 107, DateTimeKind.Utc).AddTicks(7484), new Guid("3c05c6b9-aed6-43ef-82c9-3c8457e21e59") });
        }
    }
}
