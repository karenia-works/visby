using Karenia.Visby.Account.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;


namespace Karenia.Visby.Account.Services
{
    public class AccountService
    {
        private readonly AccountContext _context;

        public AccountService(AccountContext context)
        {
            _context = context;
        }
        public async Task<LoginInfo> FindLoginInfo(string email) //TODO  根据用户的邮箱返回相应登录信息，如果没有返回空值
        {
            var tgt = await _context.LoginSessions.AsQueryable().Where(o => o.Email == email).FirstOrDefaultAsync();
            return tgt;
        }
    }
}