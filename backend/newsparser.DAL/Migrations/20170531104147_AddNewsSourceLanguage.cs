using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class AddNewsSourceLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "news_sources",
                type: "CHAR(2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FeedFormat",
                table: "news_sources",
                type: "TINYINT",
                nullable: false,
                oldClrType: typeof(char));

            migrationBuilder.CreateIndex(
                name: "IX_news_sources_Language",
                table: "news_sources",
                column: "Language");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_news_sources_Language",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "news_sources");

            migrationBuilder.AlterColumn<char>(
                name: "FeedFormat",
                table: "news_sources",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "TINYINT");
        }
    }
}
