using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class workdaysRulesChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            

            

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 11, 11, 8, 40, 256, DateTimeKind.Local).AddTicks(2558), new DateTime(2025, 11, 11, 11, 8, 40, 256, DateTimeKind.Local).AddTicks(5691), new Guid("02cbdbd0-9168-4920-b263-57634ab20104") });

            
             
 
              
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

             
            
             

           

          
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastPasswordChangedAt", "RowGuid" },
                values: new object[] { new DateTime(2025, 11, 9, 16, 9, 29, 894, DateTimeKind.Local).AddTicks(1455), new DateTime(2025, 11, 9, 14, 9, 29, 894, DateTimeKind.Utc).AddTicks(4467), new Guid("59c7808a-7ee2-4880-81c1-4e2a8e3685d7") });
        }
    }
}
