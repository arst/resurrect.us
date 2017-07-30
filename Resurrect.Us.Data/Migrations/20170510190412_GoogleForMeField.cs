using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Resurrect.Us.Data.Migrations
{
    public partial class GoogleForMeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShallBeGoogled",
                table: "ShortenedUrlRecordRecords",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShallBeGoogled",
                table: "ShortenedUrlRecordRecords");
        }
    }
}
