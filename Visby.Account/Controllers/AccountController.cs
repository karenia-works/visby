using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Account.Models;
using Karenia.Visby.Account.Services;
using Karenia.Visby.Result;

namespace Karenia.Visby.Account.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountContext context)
        {
            _service = new AccountService(context);
        }
    }
}