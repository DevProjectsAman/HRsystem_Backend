using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class employee_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Department_DepartmentId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Employee_ManagerId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Marital_Status_MaritalStatusId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Nationality_NationalityId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_Shift_ShiftId",
            //    table: "Tb_Employee");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
            //    table: "Tb_Employee");

            migrationBuilder.AlterColumn<int>(
                name: "WorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "Status",
                keyValue: null,
                column: "Status",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "SerialMobile",
                keyValue: null,
                column: "SerialMobile",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SerialMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "PrivateMobile",
                keyValue: null,
                column: "PrivateMobile",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PrivateMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "PlaceOfBirth",
                keyValue: null,
                column: "PlaceOfBirth",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceOfBirth",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "NationalId",
                keyValue: null,
                column: "NationalId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "MaritalStatusId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "HireDate",
                table: "Tb_Employee",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "EmployeeCodeHr",
                keyValue: null,
                column: "EmployeeCodeHr",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "EmployeeCodeFinance",
                keyValue: null,
                column: "EmployeeCodeFinance",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeFinance",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Tb_Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthdate",
                table: "Tb_Employee",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "ArabicLastName",
                keyValue: null,
                column: "ArabicLastName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicLastName",
                table: "Tb_Employee",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Tb_Employee",
                keyColumn: "ArabicFirstName",
                keyValue: null,
                column: "ArabicFirstName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicFirstName",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 46, 39, 881, DateTimeKind.Local).AddTicks(6188), new DateTime(2025, 9, 28, 10, 46, 39, 881, DateTimeKind.Utc).AddTicks(9580), new Guid("9ef4f009-053c-4f26-92ef-057fcc88e0ea") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Department_DepartmentId",
                table: "Tb_Employee",
                column: "DepartmentId",
                principalTable: "Tb_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Employee_ManagerId",
                table: "Tb_Employee",
                column: "ManagerId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee",
                column: "JobTitleId",
                principalTable: "Tb_Job_Title",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Marital_Status_MaritalStatusId",
                table: "Tb_Employee",
                column: "MaritalStatusId",
                principalTable: "Tb_Marital_Status",
                principalColumn: "MaritalStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Nationality_NationalityId",
                table: "Tb_Employee",
                column: "NationalityId",
                principalTable: "Tb_Nationality",
                principalColumn: "NationalityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Shift_ShiftId",
                table: "Tb_Employee",
                column: "ShiftId",
                principalTable: "Tb_Shift",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
                table: "Tb_Employee",
                column: "WorkDaysId",
                principalTable: "Tb_WorkDays",
                principalColumn: "WorkDaysId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Department_DepartmentId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Employee_ManagerId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Marital_Status_MaritalStatusId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Nationality_NationalityId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_Shift_ShiftId",
                table: "Tb_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
                table: "Tb_Employee");

            migrationBuilder.AlterColumn<int>(
                name: "WorkDaysId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedBy",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SerialMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PrivateMobile",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceOfBirth",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "NationalityId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "MaritalStatusId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "HireDate",
                table: "Tb_Employee",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeHr",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeCodeFinance",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Tb_Employee",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Tb_Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Tb_Employee",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

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

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthdate",
                table: "Tb_Employee",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicLastName",
                table: "Tb_Employee",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ArabicFirstName",
                table: "Tb_Employee",
                type: "varchar(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(55)",
                oldMaxLength: 55)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 9, 28, 13, 25, 42, 385, DateTimeKind.Local).AddTicks(8822), new DateTime(2025, 9, 28, 10, 25, 42, 386, DateTimeKind.Utc).AddTicks(2647), new Guid("f621b35b-f973-46c1-95e2-caeefbeae7da") });

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Department_DepartmentId",
                table: "Tb_Employee",
                column: "DepartmentId",
                principalTable: "Tb_Department",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Employee_ManagerId",
                table: "Tb_Employee",
                column: "ManagerId",
                principalTable: "Tb_Employee",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Job_Title_JobTitleId",
                table: "Tb_Employee",
                column: "JobTitleId",
                principalTable: "Tb_Job_Title",
                principalColumn: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Marital_Status_MaritalStatusId",
                table: "Tb_Employee",
                column: "MaritalStatusId",
                principalTable: "Tb_Marital_Status",
                principalColumn: "MaritalStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Nationality_NationalityId",
                table: "Tb_Employee",
                column: "NationalityId",
                principalTable: "Tb_Nationality",
                principalColumn: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_Shift_ShiftId",
                table: "Tb_Employee",
                column: "ShiftId",
                principalTable: "Tb_Shift",
                principalColumn: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Employee_Tb_WorkDays_WorkDaysId",
                table: "Tb_Employee",
                column: "WorkDaysId",
                principalTable: "Tb_WorkDays",
                principalColumn: "WorkDaysId");
        }
    }
}
