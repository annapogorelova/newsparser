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
                name: "news",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Author = table.Column<string>(maxLength: 100, nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DatePublished = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Guid = table.Column<string>(maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(maxLength: 255, nullable: true),
                    LinkToSource = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "news_sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateFeedUpdated = table.Column<DateTime>(nullable: false),
                    IsUpdating = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    RssUrl = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "news_tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_tags", x => x.Id);
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
                name: "news_source_news",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    NewsItemId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_source_news", x => x.Id);
                    table.ForeignKey(
                        name: "FK_news_source_news_news_NewsItemId",
                        column: x => x.NewsItemId,
                        principalTable: "news",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_news_source_news_news_sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "news_sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "news_tags_news",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    NewsItemId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_tags_news", x => x.Id);
                    table.ForeignKey(
                        name: "FK_news_tags_news_news_NewsItemId",
                        column: x => x.NewsItemId,
                        principalTable: "news",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_news_tags_news_news_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "news_tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "user_news_sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    SourceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_news_sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_news_sources_news_sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "news_sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_news_sources_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_news_source_news_NewsItemId",
                table: "news_source_news",
                column: "NewsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_news_source_news_SourceId",
                table: "news_source_news",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_news_tags_news_NewsItemId",
                table: "news_tags_news",
                column: "NewsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_news_tags_news_TagId",
                table: "news_tags_news",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_user_external_ids_UserId",
                table: "user_external_ids",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_news_sources_SourceId",
                table: "user_news_sources",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_user_news_sources_UserId",
                table: "user_news_sources",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "news_source_news");

            migrationBuilder.DropTable(
                name: "news_tags_news");

            migrationBuilder.DropTable(
                name: "user_external_ids");

            migrationBuilder.DropTable(
                name: "user_news_sources");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "news_tags");

            migrationBuilder.DropTable(
                name: "news_sources");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
