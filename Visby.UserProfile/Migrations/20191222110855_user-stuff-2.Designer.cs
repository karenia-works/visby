﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Karenia.Visby.UserProfile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Karenia.Visby.UserProfile.Migrations
{
    [DbContext(typeof(UserProfileContext))]
    [Migration("20191222110855_user-stuff-2")]
    partial class userstuff2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Karenia.Visby.UserProfile.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("money");

                    b.Property<List<int>>("DownloadList")
                        .HasColumnType("integer[]");

                    b.Property<string>("Email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<List<int>>("FavoriteList")
                        .HasColumnType("integer[]");

                    b.Property<string>("UserName")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Karenia.Visby.UserProfile.Models.UserFollow", b =>
                {
                    b.Property<int>("FollowerId")
                        .HasColumnType("integer");

                    b.Property<int>("FollowingId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FollowDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserFollows");
                });

            modelBuilder.Entity("Karenia.Visby.UserProfile.Models.UserFollow", b =>
                {
                    b.HasOne("Karenia.Visby.UserProfile.Models.User", "Follower")
                        .WithMany("Followers")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Karenia.Visby.UserProfile.Models.User", "Following")
                        .WithMany("Followings")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
