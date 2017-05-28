using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class AddIsPrivateToUserSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_sources_users_CreatorId",
                table: "news_sources");

            migrationBuilder.DropIndex(
                name: "IX_news_sources_CreatorId",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "news_sources");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "user_news_sources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "news_sources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_news_sources_UserId",
                table: "news_sources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_news_sources_users_UserId",
                table: "news_sources",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_news_sources_users_UserId",
                table: "news_sources");

            migrationBuilder.DropIndex(
                name: "IX_news_sources_UserId",
                table: "news_sources");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "user_news_sources");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "news_sources");

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "news_sources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "news_sources",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_news_sources_CreatorId",
                table: "news_sources",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_news_sources_users_CreatorId",
                table: "news_sources",
                column: "CreatorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
