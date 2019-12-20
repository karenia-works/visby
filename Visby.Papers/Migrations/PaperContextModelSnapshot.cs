﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace Karenia.Visby.Papers.Migrations
{
    [DbContext(typeof(PaperContext))]
    partial class PaperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Karenia.Visby.Papers.Models.Paper", b =>
                {
                    b.Property<int>("PaperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<List<string>>("Authors")
                        .HasColumnType("text[]");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<List<string>>("Keywords")
                        .HasColumnType("text[]");

                    b.Property<List<int>>("LocalAuthorIds")
                        .HasColumnType("integer[]");

                    b.Property<string>("PaperFrom")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<int>("PaperFromType")
                        .HasColumnType("integer");

                    b.Property<int>("QuoteCount")
                        .HasColumnType("integer");

                    b.Property<List<string>>("Quotes")
                        .HasColumnType("text[]");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .HasColumnType("tsvector");

                    b.Property<string>("Site")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("PaperId");

                    b.HasIndex("Authors")
                        .HasAnnotation("Npgsql:IndexMethod", "GIN");

                    b.HasIndex("Keywords")
                        .HasAnnotation("Npgsql:IndexMethod", "GIN");

                    b.HasIndex("SearchVector")
                        .HasAnnotation("Npgsql:IndexMethod", "GIN");

                    b.ToTable("Papers");
                });

            modelBuilder.Entity("Karenia.Visby.Papers.Models.Quote", b =>
                {
                    b.Property<int>("By")
                        .HasColumnType("integer");

                    b.Property<int>("From")
                        .HasColumnType("integer");

                    b.HasIndex("By");

                    b.HasIndex("From");

                    b.ToTable("Quotes");
                });
#pragma warning restore 612, 618
        }
    }
}
