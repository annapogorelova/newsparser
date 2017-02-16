﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsParser.DAL.Migrations
{
    public partial class AddedDateFeedUpdatedToSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateFeedUpdated",
                table: "NewsSources",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFeedUpdated",
                table: "NewsSources");
        }
    }
}
