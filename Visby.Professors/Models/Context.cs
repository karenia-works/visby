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
            // FK_PROFESSOR_APPLY
            modelBuilder.Entity<ProfessorApply>()
                .HasOne(pa => pa.Professor)
                .WithMany(p => p.Applies)
                .HasForeignKey(pa => pa.ProfessionId);
        }

        public DbSet<ProfessorApply> ProfessorApplies { set; get; }
        public DbSet<Professor> Professors { set; get; }
    }

    public class ProfessorApply
    {
        public int ProfessionApplyId { set; get; }
        public DateTime ApplyDate { set; get; }
        public int ApplyState { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        [MaxLength(2048)] public string CertificateDocument { set; get; }

        // FK_APPLY_PROFESSION
        public int ProfessionId { set; get; }
        public Professor Professor { set; get; }
    }

    public class Professor
    {
        public int ProfessionId { set; get; }
        public int UserId { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        public List<string> ReachFields { set; get; }

        // FK_APPLY_PROFESSION
        public List<ProfessorApply> Applies { set; get; }
    }
}