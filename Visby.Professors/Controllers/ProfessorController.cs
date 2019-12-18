using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Karenia.Visby.Professors.Models;
using Karenia.Visby.Professors.Services;
using Karenia.Visby.Result;
using Microsoft.AspNetCore.JsonPatch;

namespace Karenia.Visby.Professors.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ProfessorService _service;

        public ProfessorController(ProfessorService service)
        {
            _service = service;
        }

        // GET api/professor?offset={offset}&limit={limit}&keyword={keyword}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int offset, [FromQuery] int limit, [FromQuery] string keyword)
        {
            if (limit == 0)
            {
                limit = 20;
            }

            if (keyword != null)
            {
                var res = await _service.GetProfessors(keyword, limit, offset);
                bool hasNext = (res.Item1.Count != limit);
                if (res.Item3)
                    return BadRequest(new ResultList<Professor>(404, "Not Found", null, false, res.Item2, ""));
                return Ok(new ResultList<Professor>(200, null, res.Item1, hasNext, res.Item2, ""));
            }

            return NotFound();
        }

        // GET api/professor/{professor id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Result<Professor> result;
            var professor = await _service.GetProfessor(id);
            if (professor == null)
            {
                result = new Result<Professor>(404, "Professor not exists", null);
                return NotFound(result);
            }

            result = new Result<Professor>(200, null, professor);
            return Ok(result);
        }

        // POST api/professor
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Professor professor)
        {
            var result = await _service.CreateProfessor(professor);
            if (result.Item1)
                return Ok(result.Item2);
            return BadRequest(result.Item2);
        }

        // PATCH api/article/{id}
        // 传值见 https://www.cnblogs.com/lwqlun/p/10433615.html
        // 无返回值
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Professor> professor)
        {
            Professor professorResult = await _service.GetProfessor(id);
            if (professorResult == null)
                return BadRequest();
            professor.ApplyTo(professorResult);
            return Ok();
        }

        // DELETE api/professor?id={professor id}
        // 无返回值
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery(Name = "id")] int id)
        {
            var professor = await _service.GetProfessor(id);
            if (professor == null)
                return NotFound();
            _service.DeleteProfessor(professor);
            return NoContent();
        }
    }
}