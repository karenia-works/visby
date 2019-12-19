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
        [HttpGet("{id:regex([[0-9]])}")]
        public async Task<Result<Paper>> getPaper(int id)
        {
            var res = await _service.GetPaper(id);
            if (res == null)
            {
                return new Result<Paper>(400, "paper not exsit", null);
            }
            return new Result<Paper>(200, "Ok", res);
        }
        [HttpGet()]
        public async Task<ResultList<Paper>> search([FromQuery]
            List<string> keyword = null, [FromQuery]string summary = null, [FromQuery]string startTime = null,
            [FromQuery]string endTime = null, [FromQuery] List<string> author = null, [FromQuery] int skip = 0, [FromQuery]int take = 10
            )
        {
            try
            {
                var sql = _service.startSql();
                if (keyword != null)
                {
                    _service.PaperKeyword(sql, keyword);
                }
                if (summary != null)
                {
                    _service.PaperSummery(sql, summary);
                }
                if (startTime != null && endTime != null)
                {
                    _service.PaperDate(sql, DateTime.Parse(startTime), DateTime.Parse(endTime));
                }
                if (author != null)
                {
                    _service.PaperAuthor(sql, author);
                }
                var ps = await _service.GetSqlResult(sql, skip, take);
                bool hasnext = true;
                if (ps.Count == 0)
                {
                    hasnext = false;
                }
                return new ResultList<Paper>(200, "success", ps, hasnext, ps.Count, "");
            }
            catch (Exception e)
            {
                return new ResultList<Paper>(400, e.Message, null, false, 0, "");
            }
        }
        [Authorize("professorApi")]
        [HttpPost]
        public async Task<Result<Paper>> insertPaper([FromBody]Paper paper)
        {
            paper.PaperId = 0;
            var res = await _service.insertPaper(paper);
            if (res == null)
            {
                return new Result<Paper>(400, "paper exsit", null);
            }
            return new Result<Paper>(200, "O~K", res);
        }
        [HttpGet("/test")]
        public string test()
        {
            return "test";
        }

    }
}
