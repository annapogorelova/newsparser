using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class EmailConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "users");
        }
    }
}
