﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NewsParser.DAL;

namespace NewsParser.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170110135114_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NewsParser.DAL.Models.NewsCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("NewsCategories");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("LinkToSource")
                        .IsRequired();

                    b.Property<int>("SourceId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SourceId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("MainUrl")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("NewsSources");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Tags");
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

                    b.ToTable("NewsTagsNews");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Email")
                        .IsRequired()
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

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsItem", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.NewsCategory", "Category")
                        .WithMany("NewsItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.NewsSource", "Source")
                        .WithMany("News")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewsParser.DAL.Models.NewsTagsNews", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.NewsItem", "NewsItem")
                        .WithMany("Tags")
                        .HasForeignKey("NewsItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.NewsTag", "Tag")
                        .WithMany("NewsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
