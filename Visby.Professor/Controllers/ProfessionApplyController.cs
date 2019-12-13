using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Professor.Models;
using Karenia.Visby.Professor.Services;
using Karenia.Visby.Result;

namespace Karenia.Visby.Professor.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionApplyController : ControllerBase
    {
        private readonly ProfessorService _service;

        public ProfessionApplyController(ProfessorContext context)
        {
            _service = new ProfessorService(context);
        }
    }
}