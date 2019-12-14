using Karenia.Visby.Account.Models;

namespace Karenia.Visby.Account.Services
{
    public class AccountService
    {
        private readonly AccountContext _context;

        public AccountService(AccountContext context)
        {
            _context = context;
        }
    }
}