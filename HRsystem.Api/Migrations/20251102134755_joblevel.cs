using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class joblevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

             

            
           
           
          
            
            
            
            migrationBuilder.CreateIndex(
                name: "IX_Tb_Job_Level_JobLevelCode_CompanyId",
                table: "Tb_Job_Level",
                columns: new[] { "JobLevelCode", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Job_Level_JobLevelDesc_CompanyId",
                table: "Tb_Job_Level",
                columns: new[] { "JobLevelDesc", "CompanyId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tb_Job_Level_JobLevelCode_CompanyId",
                table: "Tb_Job_Level");

            migrationBuilder.DropIndex(
                name: "IX_Tb_Job_Level_JobLevelDesc_CompanyId",
                table: "Tb_Job_Level");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tb_Job_Level");

          
        }
    }
}
