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
    [Route("api/professor/[controller]")]
    [ApiController]
    public class ApplyController : ControllerBase
    {
        private readonly ProfessorApplyService _service;

        public ApplyController(ProfessorApplyService service)
        {
            _service = service;
        }
    }
}