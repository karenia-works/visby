using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Karenia.Visby.UserProfile.Models;
using Karenia.Visby.Papers.Models;

namespace Karenia.Visby.UserProfile.Models
{
    public class UserProfileContext : DbContext
    {
        public UserProfileContext(DbContextOptions<UserProfileContext> options) : base(options)
        {
            this.Database.Migrate();
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

        public DbSet<User> UserProfiles { set; get; }
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

        //用户的头像
        public string Avatar { set; get; }

        //最近下载列表
        public List<int> DownloadList { set; get; }
        public List<int> FavoriteList { set; get; }
    }


    public class UserFollow
    {
        // 关注者为follower，被关注者为following
        // 别人关注我应该显示为我的follwer，我关注的人显示为我的following
        public int FollowerId { set; get; }
        public User Follower { set; get; }
        public int FollowingId { set; get; }
        public User Following { set; get; }
        public DateTime FollowDate { set; get; }
    }
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int PaperId { get; set; }
    }
}
