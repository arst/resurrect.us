using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resurrect.Us.Data.Migrations
{
    public partial class IndexforUrlcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ResurrectRecords",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "Index_Url",
                table: "ResurrectRecords",
                column: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Url",
                table: "ResurrectRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "ResurrectRecords",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
