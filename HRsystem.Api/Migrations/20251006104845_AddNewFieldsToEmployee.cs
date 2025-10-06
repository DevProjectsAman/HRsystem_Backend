using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicFirstName",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "ArabicLastName",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Tb_Employee");

            migrationBuilder.AlterColumn<string>(
                name: "BloodGroup",
                table: "Tb_Employee",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Tb_Employee",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ArabicFullName",
                table: "Tb_Employee",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ContractTypeId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeePhotoPath",
                table: "Tb_Employee",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EnglishFullName",
                table: "Tb_Employee",
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
                values: new object[] { new DateTime(2025, 10, 6, 13, 48, 43, 952, DateTimeKind.Local).AddTicks(9931), new DateTime(2025, 10, 6, 10, 48, 43, 953, DateTimeKind.Utc).AddTicks(3327), new Guid("f530f402-2c7a-4556-bb20-93c3ea0a04f1") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "ArabicFullName",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "ContractTypeId",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "EmployeePhotoPath",
                table: "Tb_Employee");

            migrationBuilder.DropColumn(
                name: "EnglishFullName",
                table: "Tb_Employee");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "BloodGroup",
                keyValue: null,
                column: "BloodGroup",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "BloodGroup",
                table: "Tb_Employee",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ArabicFirstName",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ArabicLastName",
                table: "Tb_Employee",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 46, 39, 881, DateTimeKind.Local).AddTicks(6188), new DateTime(2025, 9, 28, 10, 46, 39, 881, DateTimeKind.Utc).AddTicks(9580), new Guid("9ef4f009-053c-4f26-92ef-057fcc88e0ea") });
        }
    }
}
