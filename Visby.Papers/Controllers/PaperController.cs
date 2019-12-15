using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Papers.Models;
using Karenia.Visby.Papers.Services;
using Karenia.Visby.Result;
using Microsoft.AspNetCore.Authorization;
namespace Karenia.Visby.Papers.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaperController : ControllerBase
    {
        private readonly PaperService _service;

        public PaperController(PaperService service)
        {
            _service = service;
        }
        [Authorize("adminApi")]
        [HttpGet("test")]
        public string test()
        {
            return "hello";
        }


    }
}