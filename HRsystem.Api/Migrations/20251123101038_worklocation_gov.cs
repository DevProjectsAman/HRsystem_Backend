using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRsystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class worklocation_gov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
          
            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Work_Location_Tb_Gov_GovId",
                table: "Tb_Work_Location",
                column: "GovId",
                principalTable: "Tb_Gov",
                principalColumn: "GovId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            
        }
    }
}
