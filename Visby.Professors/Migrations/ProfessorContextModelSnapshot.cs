﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Karenia.Visby.Professors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Karenia.Visby.Professors.Migrations
{
    [DbContext(typeof(ProfessorContext))]
    partial class ProfessorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Karenia.Visby.Professors.Models.Professor", b =>
                {
                    b.Property<int>("ProfessorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Contract")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Institution")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<List<string>>("ReachFields")
                        .HasColumnType("text[]");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ProfessorId");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("Karenia.Visby.Professors.Models.ProfessorApply", b =>
                {
                    b.Property<int>("ProfessorApplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("ApplyDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ApplyState")
                        .HasColumnType("integer");

                    b.Property<string>("CertificateDocument")
                        .HasColumnType("character varying(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("Contract")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Institution")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<int>("ProfessorId")
                        .HasColumnType("integer");

                    b.HasKey("ProfessorApplyId");

                    b.HasIndex("ProfessorId");

                    b.ToTable("ProfessorApplies");
                });

            modelBuilder.Entity("Karenia.Visby.Professors.Models.ProfessorApply", b =>
                {
                    b.HasOne("Karenia.Visby.Professors.Models.Professor", "Professor")
                        .WithMany("Applies")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
