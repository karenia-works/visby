using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Professors.Models;
using Karenia.Visby.Professors.Services;
using Karenia.Visby.Result;

namespace Karenia.Visby.Professors.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private readonly ProfessorService _service;

        public ProfessionController(ProfessorContext context)
        {
            _service = new ProfessorService(context);
        }
    }
}