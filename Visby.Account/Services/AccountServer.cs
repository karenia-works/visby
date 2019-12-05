using Karenia.Visby.Account.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
namespace Karenia.Visby.Account.Services
{
    public class AccountServer
    {
        AccountContext db;
        public AccountServer(AccountContext accountContext)
        {
            db = accountContext;
        }
        public async Task<LoginInfo> findLoginInfo(string email)//TODO  根据用户的邮箱返回相应登录信息，如果没有返回空值
        {
            var tgt = await db.LoginSessions.AsQueryable().Where(o => o.Email == email).FirstOrDefaultAsync();
            return tgt;
        }
    }
}