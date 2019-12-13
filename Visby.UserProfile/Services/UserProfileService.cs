using Karenia.Visby.UserProfile.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using Karenia.Visby.UserProfile.Models;

namespace Karenia.Visby.UserProfile.Services
{
    public class UserProfileService
    {
        private readonly UserProfileContext _context;

        public UserProfileService(UserProfileContext userProfileContext)
        {
            _context = userProfileContext;
        }

        public async Task<User> GetUserProfile(int id)
        {
            return await _context.UserProfiles
                .Where(p => p.UserId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUserProfilesKeyword(string keyword)
        {
            return await _context.UserProfiles
                .Where(up => up.UserName.Contains(keyword))
                .ToListAsync();
        }

        public async Task<User> GetUserProfileEmail(string email)
        {
            return await _context.UserProfiles.AsQueryable().Where(p => p.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetFollowers(int id)
        {
            // 获取主动关注当前用户的人
            return await _context.UserFollows
                .Where(uf => uf.FollowerId == id)
                .Select(uf => _context.UserProfiles
                    .FirstOrDefault(up => up.UserId == uf.FollowingId))
                .ToListAsync();
        }

        public async Task<List<User>> GetFollowings(int id)
        {
            // 获取当前用户主动关注的人
            return await _context.UserFollows
                .Where(uf => uf.FollowerId == id)
                .Select(uf => _context.UserProfiles
                    .FirstOrDefault(up => up.UserId == uf.FollowerId))
                .ToListAsync();
        }
    }
}