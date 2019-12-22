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
                var sql = _service._context.Papers.AsQueryable();

                if (summary != null)
                {
                    sql = _service.PaperSummery(sql, summary);
                }
                if (startTime != null && endTime != null)
                {
                    sql = _service.PaperDate(sql, DateTime.Parse(startTime), DateTime.Parse(endTime));
                }
                bool hasnext = true;
                if (keyword.Count != 0 || author.Count != 0)
                {

                    var newsql = sql.ToAsyncEnumerable();

                    if (keyword.Count != 0)
                    {

                        newsql = _service.PaperKeyword(newsql, keyword);
                    }
                    if (author.Count != 0)
                    {

                        newsql = _service.PaperAuthor(newsql, author);
                    }
                    var eps = await _service.GetSqlResult(newsql, skip, take);
                    if (eps.Count == 0)
                    {
                        hasnext = false;
                    }
                    return new ResultList<Paper>(200, "success", eps, hasnext, eps.Count, "");
                }

                var ps = await _service.GetSqlResult(sql, skip, take);

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
        // [Authorize("professorApi")]
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
        [HttpGet("papers")]
        public async Task<ResultList<Paper>> findPapers([FromQuery] List<int> tgt = null)
        {
            var res = await _service.GetPapers(tgt);
            return new ResultList<Paper>(200, "Ko", res, false, 0, "");
        }
        [HttpGet("test")]
        public async Task<ResultList<Paper>> test()
        {
            // var sql = _service.startSql();
            // sql = _service.PaperSummery(sql, "燃烧");
            // var res = await _service.GetSqlResult(sql, 0, 10);
            var res = await _service.test();
            return new ResultList<Paper>(200, "O~K", res, false, res.Count, null);
        }

    }
}
