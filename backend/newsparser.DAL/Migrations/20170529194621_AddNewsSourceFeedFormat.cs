using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class AddNewsSourceFeedFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RssUrl",
                table: "news_sources",
                newName: "FeedUrl");

            migrationBuilder.AddColumn<char>(
                name: "FeedFormat",
                table: "news_sources",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "news_sources",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_news_sources_FeedFormat",
                table: "news_sources",
                column: "FeedFormat");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedUrl",
                table: "news_sources",
                newName: "RssUrl");

            migrationBuilder.DropIndex(
                name: "IX_news_sources_FeedFormat",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "FeedFormat",
                table: "news_sources");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "news_sources",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
