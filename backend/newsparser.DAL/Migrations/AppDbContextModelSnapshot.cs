using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NewsParser.DAL;
using newsparser.DAL.Models;

namespace newsparser.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("NewsParser.DAL.Models.NewsItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageUrl");

                    b.Property<string>("LinkToSource")
                        .IsRequired();

                    b.Property<int>("SourceId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.ToTable("news");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateFeedUpdated");

                    b.Property<bool>("IsUpdating");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("RssUrl")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("news_sources");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("news_tags");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsTagsNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NewsItemId");

                    b.Property<int>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("NewsItemId");

                    b.HasIndex("TagId");

                    b.ToTable("news_tags_news");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(2147483647);

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("newsparser.DAL.Models.UserExternalId", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthProvider");

                    b.Property<string>("ExternalId")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_external_ids");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.UserNewsSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SourceId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SourceId");

                    b.HasIndex("UserId");

                    b.ToTable("user_sources");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsItem", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.NewsSource", "Source")
                        .WithMany("News")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsTagsNews", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.NewsItem", "NewsItem")
                        .WithMany("NewsItemTags")
                        .HasForeignKey("NewsItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.NewsTag", "Tag")
                        .WithMany("TagNewsItems")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("newsparser.DAL.Models.UserExternalId", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.User", "User")
                        .WithMany("UserExternalIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewsParser.DAL.Models.UserNewsSource", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.NewsSource", "NewsSource")
                        .WithMany("Users")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.User", "User")
                        .WithMany("NewsSources")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
