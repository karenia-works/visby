using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using NpgsqlTypes;
using Npgsql.EntityFrameworkCore;
namespace Karenia.Visby.Papers.Models
{
    public class PaperContext : DbContext
    {
        public PaperContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paper>().HasIndex(p => p.SearchVector).HasMethod("GIN");
            modelBuilder.Entity<Paper>().HasIndex(p => p.Authors).HasMethod("GIN");
            modelBuilder.Entity<Paper>().HasIndex(p => p.Keywords).HasMethod("GIN");
            modelBuilder.Entity<Paper>().HasKey(p => p.PaperId);
            modelBuilder.Entity<Quote>().HasNoKey();
            modelBuilder.Entity<Quote>().HasIndex(p => p.By);
            modelBuilder.Entity<Quote>().HasIndex(p => p.From);
        }

        public DbSet<Paper> Papers { set; get; }
        public DbSet<Quote> Quotes { set; get; }
    }

    public class Paper
    {
        public int PaperId { set; get; }
        [MaxLength(128)] public string Title { get; set; }
        public List<string> Authors { set; get; }
        public int Type { set; get; }
        public string Summary { get; set; }
        public int PaperFromType { get; set; }
        [MaxLength(128)] public string PaperFrom { get; set; }
        [MaxLength(64)] public string Site { get; set; }
        public DateTime Date { get; set; }
        public List<String> Keywords { get; set; }
        public int QuoteCount { get; set; }
        public List<String> Quotes { get; set; }
        // 本站收录的专家
        // 跨库无法建外键
        public List<int> LocalAuthorIds { set; get; }
        public NpgsqlTsVector SearchVector { set; get; }
    }
    public class Quote
    {
        public int From { set; get; }
        public int By { set; get; }

    }
}