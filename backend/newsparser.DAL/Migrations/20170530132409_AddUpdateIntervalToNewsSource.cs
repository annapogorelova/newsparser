using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class AddUpdateIntervalToNewsSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastBuildDate",
                table: "news_sources");

            migrationBuilder.AddColumn<int>(
                name: "UpdateIntervalMinutes",
                table: "news_sources",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.CreateIndex(
                name: "IX_news_sources_FeedUrl",
                table: "news_sources",
                column: "FeedUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_news_sources_FeedUrl",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "UpdateIntervalMinutes",
                table: "news_sources");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastBuildDate",
                table: "news_sources",
                nullable: true);
        }
    }
}
