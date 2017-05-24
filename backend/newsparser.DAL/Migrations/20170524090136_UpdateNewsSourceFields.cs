using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class UpdateNewsSourceFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "news_sources",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "news_sources",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastBuildDate",
                table: "news_sources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "news_sources",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "LastBuildDate",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "news_sources");
        }
    }
}
