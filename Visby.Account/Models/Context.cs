using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Karenia.Visby.Account.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<User> Users { set; get; }
    }

    public class User
    {
        public int UserId { set; get; }
        [MaxLength(256)] public string Email { set; get; }
        [MaxLength(256)] public string Password { set; get; }
    }
}