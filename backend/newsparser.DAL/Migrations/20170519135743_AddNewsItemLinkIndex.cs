using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class AddNewsItemLinkIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_news_LinkToSource",
                table: "news",
                column: "LinkToSource");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_news_LinkToSource",
                table: "news");
        }
    }
}
