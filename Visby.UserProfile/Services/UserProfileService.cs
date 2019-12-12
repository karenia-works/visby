using Karenia.Visby.UserProfile.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace Kiruna.Visby.UserProfole.Services
{
    public class UserProfileService
    {
        UserProfileContext db;
        public UserProfileService(UserProfileContext userProfileContent)
        {
            db = userProfileContent;
        }
        public async Task<UserProfile> GetUserProfile(int id)
        {
            var p = await db.Users.AsQueryable().Where(p => p.UserId == id).FirstOrDefaultAsync();
            return p;
        }

        public async Task<List<UserProfile>> GetUserProfilesKeyword(String Keywords)
        {
            var p = await db.Users.FromSqlRaw("Select * from Users where UserName like '{0}'", Keywords).ToListAsync();
            return p;
        }

        public async Task<UserProfile> GetUserProfileEmail(string email)
        {
            var p = await db.Users.AsQueryable().Where(p => p.Email == email).FirstOrDefaultAsync();
            return p;
        }

        public async Task<List<UserProfile>> GetFollowers(int id)
        {
            //var p = await db.Users.AsQueryable().Where(p => p.UserId == id).FirstOrDefaultAsync();
            var p = await db.Users.FromSqlRaw("select Followers from Users where UserId='{0}'", id).ToListAsync();
            return p;
        }

        public async Task<List<UserProfile>> GetFollowings(int id)
        {
            //var p = await db.Users.AsQueryable().Where(p => p.UserId == id).FirstOrDefaultAsync();
            var p = await db.Users.FromSqlRaw("select Followings from Users where UserId='{0}'", id).ToListAsync();
            return p;
        }
    }
}