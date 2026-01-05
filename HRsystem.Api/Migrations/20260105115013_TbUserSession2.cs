using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class TbUserSession2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_UserSessions_AspNetUsers_UserId",
                table: "Tb_UserSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_UserSessions",
                table: "Tb_UserSessions");

            migrationBuilder.RenameTable(
                name: "Tb_UserSessions",
                newName: "Tb_User_Sessions");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_UserSessions_UserId",
                table: "Tb_User_Sessions",
                newName: "IX_Tb_User_Sessions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_User_Sessions",
                table: "Tb_User_Sessions",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 5, 11, 50, 12, 844, DateTimeKind.Utc).AddTicks(9401), new DateTime(2026, 1, 5, 11, 50, 12, 845, DateTimeKind.Utc).AddTicks(2452), new Guid("eca2cb30-e800-4226-92a2-203e8feecc46") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_User_Sessions_AspNetUsers_UserId",
                table: "Tb_User_Sessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_User_Sessions_AspNetUsers_UserId",
                table: "Tb_User_Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_User_Sessions",
                table: "Tb_User_Sessions");

            migrationBuilder.RenameTable(
                name: "Tb_User_Sessions",
                newName: "Tb_UserSessions");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_User_Sessions_UserId",
                table: "Tb_UserSessions",
                newName: "IX_Tb_UserSessions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_UserSessions",
                table: "Tb_UserSessions",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2026, 1, 5, 11, 47, 47, 704, DateTimeKind.Utc).AddTicks(2437), new DateTime(2026, 1, 5, 11, 47, 47, 704, DateTimeKind.Utc).AddTicks(5601), new Guid("14990f74-adff-4234-ac2c-5038cecf2889") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_UserSessions_AspNetUsers_UserId",
                table: "Tb_UserSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
