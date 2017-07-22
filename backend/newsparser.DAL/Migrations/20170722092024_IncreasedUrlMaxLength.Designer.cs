using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NewsParser.DAL;
using NewsParser.DAL.Models;
using newsparser.DAL.Models;

namespace newsparser.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170722092024_IncreasedUrlMaxLength")]
    partial class IncreasedUrlMaxLength
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("NewsParser.DAL.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateFeedUpdated");

                    b.Property<string>("Description")
                        .HasMaxLength(255);

                    b.Property<int>("FeedFormat")
                        .HasColumnType("TINYINT");

                    b.Property<string>("FeedUrl")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500);

                    b.Property<bool>("IsUpdating");

                    b.Property<string>("Language")
                        .HasColumnType("CHAR(2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("UpdateIntervalMinutes");

                    b.Property<int?>("UserId");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("FeedFormat");

                    b.HasIndex("FeedUrl");

                    b.HasIndex("Language");

                    b.HasIndex("UserId");

                    b.ToTable("channels");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.ChannelFeedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelId");

                    b.Property<int>("FeedItemId");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("FeedItemId");

                    b.ToTable("channels_feed_items");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.FeedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .HasMaxLength(100);

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DatePublished");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500);

                    b.Property<string>("LinkToSource")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("LinkToSource");

                    b.ToTable("feed_items");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.TagFeedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FeedItemId");

                    b.Property<int>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("FeedItemId");

                    b.HasIndex("TagId");

                    b.ToTable("tags_feed_items");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.Token", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("tokens");
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

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(2147483647);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.UserChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelId");

                    b.Property<bool>("IsPrivate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("UserId");

                    b.ToTable("user_channels");
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

            modelBuilder.Entity("NewsParser.DAL.Models.Channel", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.User")
                        .WithMany("CreatedChannels")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NewsParser.DAL.Models.ChannelFeedItem", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.Channel", "Channel")
                        .WithMany("Feed")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.FeedItem", "FeedItem")
                        .WithMany("Channels")
                        .HasForeignKey("FeedItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewsParser.DAL.Models.TagFeedItem", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.FeedItem", "FeedItem")
                        .WithMany("Tags")
                        .HasForeignKey("FeedItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.Tag", "Tag")
                        .WithMany("Feed")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NewsParser.DAL.Models.UserChannel", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.Channel", "Channel")
                        .WithMany("Users")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NewsParser.DAL.Models.User", "User")
                        .WithMany("Channels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("newsparser.DAL.Models.UserExternalId", b =>
                {
                    b.HasOne("NewsParser.DAL.Models.User", "User")
                        .WithMany("UserExternalIds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
