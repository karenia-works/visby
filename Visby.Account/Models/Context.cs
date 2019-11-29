using System;
using System.Collections.Generic;
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

        public DbSet<User> Users { set; get; }
        public DbSet<LoginInfo> LoginSessions { set; get; }
        public DbSet<ProfessionApply> ProfessionApplies { set; get; }
        public DbSet<UserFollow> UserFollows { set; get; }
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
}