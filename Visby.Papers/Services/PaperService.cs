using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace Visby.Papers.Services
{
    public class PaperService
    {
        PaperContext db;
        public PaperService(PaperContext paperContext)
        {
            db = paperContext;
        }
        public async Task<Paper> getPaper(int id)
        {
            var p = await db.Papers.AsQueryable().Where(p => p.PaperId == id).FirstOrDefaultAsync();
            return p;
        }
        public async Task<List<Paper>> getPaperKeyword(List<String> Keywords)
        {
            var ps = await db.Papers.FromSqlRaw("select * from Papers where keyword <@ '{0}'", Keywords).ToListAsync();
            return ps;
        }
        public async Task<List<Paper>> GetPaperAuthor(List<String> authors)
        {
            var ps = await db.Papers.FromSqlRaw("select * from Papers where Author <@ '{0}'", authors).ToListAsync();
            return ps;
        }
        public async Task<List<Paper>> GetPaperLocalAuthor(List<int> Localauthers)
        {
            var ps = await db.Papers.FromSqlRaw("select * from Papers where Localauthor <@'{0}'", Localauthers).ToListAsync();
            return ps;
        }
        public async Task<List<Paper>> GetPaperSummery(String tgt)//记得 加索引
        {
            var ps = await db.Papers.FromSqlRaw("select *,MAX(similarity({0},Title),similarity({0},summary)) As sml from paper where (summary % '{0}') or (summary%{0}) order by sml,quote DESC", tgt).ToListAsync();
            return ps;
        }
        public async Task<List<Paper>> GetPapers
    }
}