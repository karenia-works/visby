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
using Microsoft.AspNetCore.JsonPatch;

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

        // GET api/apply?offset={offset}&limit={limit}&id={professor id}
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int offset, [FromQuery] int limit, [FromQuery] int id)
        {
            if (limit == 0)
            {
                limit = 20;
            }

            if (id != 0)
            {
                var res = await _service.GetApplies(id, limit, offset);
                bool hasNext = (res.Item1.Count != limit);
                return Ok(new ResultList<ProfessorApply>(200, null, res.Item1, hasNext, res.Item2, ""));
            }

            return NotFound();
        }

        // GET api/apply/{apply id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Result<ProfessorApply> result;
            var apply = await _service.GetApply(id);
            if (apply == null)
            {
                result = new Result<ProfessorApply>(404, "Professor not exists", null);
                return NotFound(result);
            }

            result = new Result<ProfessorApply>(200, null, apply);
            return Ok(result);
        }

        // POST api/professor
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProfessorApply apply)
        {
            var result = await _service.CreateApply(apply);
            if (result.Item1)
                return Ok(result.Item2);
            return BadRequest(result.Item2);
        }

        // PATCH api/article/{id}
        // 传值见 https://www.cnblogs.com/lwqlun/p/10433615.html
        // 无返回值
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<ProfessorApply> apply)
        {
            ProfessorApply applyResult = await _service.GetApply(id);
            if (applyResult == null)
                return BadRequest();
            apply.ApplyTo(applyResult);
            return Ok();
        }

        // DELETE api/professor?id={professor id}
        // 无返回值
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery(Name = "id")] int id)
        {
            var professor = await _service.GetApply(id);
            if (professor == null)
                return NotFound();
            _service.DeleteApply(professor);
            return NoContent();
        }
    }
}
