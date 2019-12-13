using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Karenia.Visby.Papers.Models
{
    public class PaperContext : DbContext
    {
        public PaperContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paper>()
                .HasKey(p => p.PaperId);
        }

        public DbSet<Paper> Papers { set; get; }
    }

    public class Paper
    {
        public int PaperId { set; get; }
        [MaxLength(128)] public string Title { get; set; }
        public List<string> Author { set; get; }
        public List<int> LocalAuthor { set; get; }
        public int Type { set; get; }
        public string Summary { get; set; }
        public int PaperFromType { get; set; }
        [MaxLength(128)] public string PaperFrom { get; set; }
        [MaxLength(64)] public string Site { get; set; }
        public DateTime Date { get; set; }
        public List<String> Key { get; set; }
        public int Quote { get; set; }
    }
}