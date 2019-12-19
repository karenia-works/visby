using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Karenia.Visby.UserProfile.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions options) : base(options)
        {
            this.Database.Migrate();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasIndex(li => li.Email)
                .IsUnique();
            // PK_LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasKey(li => li.UserId);
        }

        public DbSet<LoginInfo> LoginSessions { set; get; }
    }

    public class LoginInfo
    {
        public int UserId { set; get; }
        [MaxLength(256)] public string Email { set; get; }
        [MaxLength(256)] public string EncryptedPassword { set; get; }
        [StringLength(32)] public string Key { set; get; }
        public int Type { set; get; }

        public void hashMyPassword()
        {
            var key = new byte[32];
            RandomNumberGenerator.Fill(key);
            var hashed = new HMACSHA256(key).ComputeHash(new UTF8Encoding().GetBytes(this.EncryptedPassword));
            this.EncryptedPassword = System.Convert.ToBase64String(hashed);
            this.Key = System.Convert.ToBase64String(key);
        }

        public bool checkPassword(string incoming)
        {
            var key = System.Convert.FromBase64String(this.Key);
            var hashed = new HMACSHA256(key).ComputeHash(new UTF8Encoding().GetBytes(incoming));
            var realPassword = System.Convert.FromBase64String(this.EncryptedPassword);
            Console.WriteLine(System.Convert.ToBase64String(hashed));
            return slowByteEq(realPassword, hashed);
        }

        private static bool slowByteEq(byte[] byteArray, byte[] other)
        {
            if (byteArray.Length != other.Length) return false;
            var eq = true;
            for (int i = 0; i < byteArray.Length; i++)
            {
                eq = eq && byteArray[i] == other[i];
            }

            return eq;
        }
    }
}
