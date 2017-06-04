using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace newsparser.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feed_items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Author = table.Column<string>(maxLength: 100, nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DatePublished = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Guid = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 255, nullable: true),
                    LinkToSource = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feed_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Subject = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 2147483647, nullable: false),
                    UserName = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags_feed_items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    FeedItemId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags_feed_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tags_feed_items_feed_items_FeedItemId",
                        column: x => x.FeedItemId,
                        principalTable: "feed_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tags_feed_items_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateFeedUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    FeedFormat = table.Column<int>(type: "TINYINT", nullable: false),
                    FeedUrl = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 255, nullable: true),
                    IsUpdating = table.Column<bool>(nullable: false),
                    Language = table.Column<string>(type: "CHAR(2)", nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    UpdateIntervalMinutes = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    WebsiteUrl = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_channels_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_external_ids",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AuthProvider = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_external_ids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_external_ids_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "channels_feed_items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ChannelId = table.Column<int>(nullable: false),
                    FeedItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels_feed_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_channels_feed_items_channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_channels_feed_items_feed_items_FeedItemId",
                        column: x => x.FeedItemId,
                        principalTable: "feed_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ChannelId = table.Column<int>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_channels_channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_channels_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channels_FeedFormat",
                table: "channels",
                column: "FeedFormat");

            migrationBuilder.CreateIndex(
                name: "IX_channels_FeedUrl",
                table: "channels",
                column: "FeedUrl");

            migrationBuilder.CreateIndex(
                name: "IX_channels_Language",
                table: "channels",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_channels_UserId",
                table: "channels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_channels_feed_items_ChannelId",
                table: "channels_feed_items",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_channels_feed_items_FeedItemId",
                table: "channels_feed_items",
                column: "FeedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_feed_items_LinkToSource",
                table: "feed_items",
                column: "LinkToSource");

            migrationBuilder.CreateIndex(
                name: "IX_tags_Name",
                table: "tags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_tags_feed_items_FeedItemId",
                table: "tags_feed_items",
                column: "FeedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_tags_feed_items_TagId",
                table: "tags_feed_items",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_user_channels_ChannelId",
                table: "user_channels",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_user_channels_UserId",
                table: "user_channels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_external_ids_UserId",
                table: "user_external_ids",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channels_feed_items");

            migrationBuilder.DropTable(
                name: "tags_feed_items");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "user_channels");

            migrationBuilder.DropTable(
                name: "user_external_ids");

            migrationBuilder.DropTable(
                name: "feed_items");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
