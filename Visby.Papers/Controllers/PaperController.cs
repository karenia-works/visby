﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Papers.Models;
using Karenia.Visby.Papers.Services;
using Karenia.Visby.Result;

namespace Karenia.Visby.Papers.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly PaperService _service;

        public ProfessionController(PaperContext context)
        {
            _service = new PaperService(context);
        }
    }
}