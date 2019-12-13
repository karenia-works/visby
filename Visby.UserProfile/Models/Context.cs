using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Karenia.Visby.UserProfile.Models
{
    public class UserProfileContext : DbContext
    {
        public UserProfileContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: FK and UniqueKey
            // USER
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<UserProfile>()
                .Property(u => u.Balance)
                .HasColumnType("money");

            // LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasIndex(li => li.Email)
                .IsUnique();
            // PK_LOGIN_INFO
            modelBuilder.Entity<LoginInfo>()
                .HasKey(li => li.UserId);

            // FOLLOW
            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new {uf.FollowerId, uf.FollowingId});
            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Following)
                .WithMany(fg => fg.Followings)
                .HasForeignKey(uf => uf.FollowingId);
            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(fer => fer.Followers)
                .HasForeignKey(uf => uf.FollowerId);
        }

        public DbSet<UserProfile> Users { set; get; }
        public DbSet<LoginInfo> LoginSessions { set; get; }
        public DbSet<UserFollow> UserFollows { set; get; }
    }

    public class UserProfile
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

    public class UserFollow
    {
        // 关注者为follower，被关注者为following
        // 别人关注我应该显示为我的follwer，我关注的人显示为我的following
        public int FollowerId { set; get; }
        public UserProfile Follower { set; get; }
        public int FollowingId { set; get; }
        public UserProfile Following { set; get; }
        public DateTime FollowDate { set; get; }
    }
}