using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Karenia.Visby.UserProfile.Models;
using Karenia.Visby.Result;
using Microsoft.AspNetCore.JsonPatch;
using Karenia.Visby.Papers.Models;

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
            return await _context.UserProfiles.AsQueryable()
                .Where(p => p.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUserProfilesKeyword(string keyword)
        {
            return await _context.UserProfiles.AsQueryable()
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
            return await _context.UserFollows.AsQueryable()
                .Where(uf => uf.FollowerId == id)
                .Select(uf => _context.UserProfiles
                    .FirstOrDefault(up => up.UserId == uf.FollowingId))
                .ToListAsync();
        }

        public async Task<List<User>> GetFollowings(int id)
        {
            // 获取当前用户主动关注的人
            return await _context.UserFollows.AsQueryable()
                .Where(uf => uf.FollowerId == id)
                .Select(uf => _context.UserProfiles
                    .FirstOrDefault(up => up.UserId == uf.FollowerId))
                .ToListAsync();
        }

        public async Task<Tuple<int, string>> InsertUserProfile(User user_in){
            // Tuple<bool, Result<User>> result;
            // if(_context.UserProfiles.Any(uf => uf.Email == user_in.Email)){
            //     result = new Tuple<bool, Result<User>>(false,
            //         new Result<User>(400, "User email already occupied", null));

            //     return result;
            // }

            //id置0
            user_in.UserId = 0;
            
            Tuple<int, string> result;

            try{
                await _context.AddAsync(user_in);
                await _context.SaveChangesAsync();
            }
            catch(Exception e){
                result = Tuple.Create(400, "insert failed");
                return result;
            }

            // result = new Tuple<bool, Result<User>>(true, new Result<User>(200, null, user_in));
            result = Tuple.Create(200, "insert successfully");
            return result;
        }

        //public async Task<Tuple<int, string>> DeleteUserProfile(User user){
        public async void DeleteUserProfile(User user){
            // Tuple<int, string> result;

            // try{
            //     _context.UserProfiles.Remove(user);
            //     await _context.SaveChangesAsync();
            // }
            // catch(Exception e){
            //     result = Tuple.Create(400, "delete failed");
            //     return result;
            // }
            // result = Tuple.Create(200, "delete successfully");
            // return result;

            _context.UserProfiles.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<int, string>> UpdateUserProfile(User user_in){
            Tuple<int, string> result;
            if(!_context.UserProfiles.Any(uf => uf.UserId == user_in.UserId)){
                result = new Tuple<int, string>(404,"User Profile do not exist");
                return result;
            }

            try{
                _context.UserProfiles.Update(user_in);
                await _context.SaveChangesAsync();
            }
            catch(Exception e){
                result = Tuple.Create(400, "update failed");
                return result;
            }

            result = Tuple.Create(200, "update successfully");
            return result;
        }

        public async Task<List<int>> GetDownLoadList(string id_in){
            int user_id = Convert.ToInt32(id_in);
            if(!_context.UserProfiles.Any(up => up.UserId == user_id)){
                return null;
            }
            var query = await _context.UserProfiles.AsQueryable().Where(up => up.UserId == user_id).FirstOrDefaultAsync();
            return query.DownloadList;
        }

        public async Task<List<User>> GetUserProfileList(){
            var query = await _context.UserProfiles.AsQueryable().OrderBy(up => up.UserId).ToListAsync();
            return query;
        }
        
    }
}