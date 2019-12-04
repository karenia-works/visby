using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Karenia.Visby.Professor.Models
{
    public class ProfessorContext : DbContext
    {
        public ProfessorContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: FK and UniqueKey
            // USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasColumnType("money");

            // LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasIndex(li => li.Email)
                .IsUnique();
            // PK_LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasKey(li => li.UserId);

            // PROFESSION_APPLY
            modelBuilder.Entity<ProfessionApply>()
                .HasKey(pa => pa.ApplyId);

            // FOLLOW
            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FollowingId });
            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Following)
                .WithMany(fg => fg.Followings)
                .HasForeignKey(uf => uf.FollowingId);
            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(fer => fer.Followers)
                .HasForeignKey(uf => uf.FollowerId);
        }

        public DbSet<User> Users { set; get; }
        public DbSet<LoginInfo> LoginSessions { set; get; }
        public DbSet<ProfessionApply> ProfessionApplies { set; get; }
        public DbSet<UserFollow> UserFollows { set; get; }
        public DbSet<Profession> Professions { set; get; }
    }

    public class User
    {
        public int UserId { set; get; }
        [MaxLength(256)] public string Email { set; get; }
        [MaxLength(256)] public string UserName { set; get; }
        public decimal Balance { set; get; }

        // FK_USER_USER_FOLLOW
        public List<UserFollow> Followers { set; get; }
        public List<UserFollow> Followings { set; get; }
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

    public class ProfessionApply
    {
        public int ApplyId { set; get; }
        public DateTime ApplyDate { set; get; }
        public int ApplyState { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        [MaxLength(2048)] public string CertificateDocument { set; get; }
    }

    public class UserFollow
    {
        // 关注者为follower，被关注者为following
        public int FollowerId { set; get; }
        public User Follower { set; get; }
        public int FollowingId { set; get; }
        public User Following { set; get; }
        public DateTime FollowDate { set; get; }
    }

    public class Profession
    {
        public int ProfessionId { set; get; }
        public int UserId { set; get; }
        [MaxLength(128)] public string Name { set; get; }
        [MaxLength(128)] public string Contract { set; get; }
        [MaxLength(128)] public string Institution { set; get; }
        public List<string> ReachFields { set; get; }
    }
}