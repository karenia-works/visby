using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.UserProfile.Models;

namespace Karenia.Visby.UserProfile.Services
{
    public class AccountService
    {
        private readonly AccountContext _context;
        public AccountService(AccountContext accountContext){
            _context = accountContext;
        }

        public async Task<bool> insertLoginInfo(LoginInfo loginInfo){
            try{
                await _context.AddAsync(loginInfo);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        //public async Task<bool> deleteLoginInfo(LoginInfo loginInfo){
        public async void deleteLoginInfo(LoginInfo loginInfo){
            // try{
            //     _context.LoginSessions.Remove(loginInfo);
            //     await _context.SaveChangesAsync();
            // }
            // catch(Exception e)
            // {
            //     return false;
            // }
            // return  true;
            _context.LoginSessions.Remove(loginInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<Tuple<int, string>> updateLoginInfo(LoginInfo loginInfo){
            Tuple<int, string> result;
                if(!_context.LoginSessions.Any(ls => ls.UserId == loginInfo.UserId)){
                result = new Tuple<int, string>(404,"account do not exist");
                return result;
            }

            try{
                _context.LoginSessions.Update(loginInfo);
                await _context.SaveChangesAsync();
            }
            catch(Exception e){
                result = Tuple.Create(400, "update failed");
                return result;
            }

            result = Tuple.Create(200, "update successfully");
            return result;
        }

        public async Task<LoginInfo> GetLoginInfo(int id)
        {
            return await _context.LoginSessions.AsQueryable()
                .FirstOrDefaultAsync(p => p.UserId == id);
        }
    }
}