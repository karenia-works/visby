using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Karenia.Visby.Professors.Models
{
    public class ProfessorContext : DbContext
    {
        public ProfessorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FK_APPLY_PROFESSOR
            modelBuilder.Entity<ProfessorApply>()
                .HasOne(pa => pa.Professor)
                .WithMany(p => p.Applies)
                .HasForeignKey(pa => pa.ProfessorId);
        }

        public DbSet<ProfessorApply> ProfessorApplies { set; get; }
        public DbSet<Professor> Professors { set; get; }
    }

    public class ProfessorApply
    {
        public int ProfessorApplyId { set; get; }
        public DateTime ApplyDate { set; get; }
        public int ApplyState { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        [MaxLength(2048)] public string CertificateDocument { set; get; }

        // FK_APPLY_PROFESSOR
        public int ProfessorId { set; get; }
        public Professor Professor { set; get; }
    }

    public class Professor
    {
        public int ProfessorId { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        public List<string> ReachFields { set; get; }

        // FK_APPLY_PROFESSION
        public List<ProfessorApply> Applies { set; get; }

        // 跨库无法建外键
        public int UserId { set; get; }
    }
}