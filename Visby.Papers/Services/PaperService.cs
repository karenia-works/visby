using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql.EntityFrameworkCore;
namespace Karenia.Visby.Papers.Services
{
    public class PaperService
    {
        private readonly PaperContext _context;

        public PaperService(PaperContext paperContext)
        {
            _context = paperContext;
            paperContext.Database.ExecuteSqlRaw("CREATE EXTENSION pg_trgm");
            paperContext.Database.ExecuteSqlRaw("CREATE EXTENSION pg_jieba");
        }

        public async Task<Paper> GetPaper(int id)
        {
            return await _context.Papers
                .Where(p => p.PaperId == id)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Paper> PaperKeyword(IQueryable<Paper> sql, List<String> Keywords)
        {
            return sql.Where(p => p.Keywords.All(i => Keywords.Contains(i)));
            /*var ps = await _context.Papers.FromSqlRaw("select * from Papers where keyword <@ '{0}'", Keywords)
                .ToListAsync();
            return ps;*/
        }
        public List<Paper> testdata()
        {
            return _context.Papers.AsQueryable().ToList();

        }

        public IQueryable<Paper> PaperAuthor(IQueryable<Paper> sql, List<String> authors)
        {
            return sql.Where(p => p.Authors.All(i => authors.Contains(i)));

            /*var ps = await _context.Papers.FromSqlRaw("select * from public.\"Papers\" where \"Authors\" @> array[{0}]", authors)
                .ToListAsync();
            return ps;*/
        }
        public IQueryable<Paper> PaperDate(IQueryable<Paper> sql, DateTime BeginDate, DateTime EndDate)
        {
            return sql.Where(p => p.Date < EndDate && p.Date > BeginDate);
        }
        public async Task<List<Paper>> PaperLocalAuthor(List<int> Localauthers)
        {

            var ps = await _context.Papers.FromSqlRaw("select * from Papers where Localauthor <@'{0}'", Localauthers)
                .ToListAsync();
            return ps;
        }
        public IQueryable<Paper> startSql()
        {
            return _context.Papers.Select(o => o);
        }
        public async Task<List<Paper>> GetSqlResult(IQueryable<Paper> sql, int Skip, int take)
        {

            var ps = await sql.OrderBy(p => p.Quote).Skip(Skip).Take(take).ToListAsync();
            return ps;
        }
        public IQueryable<Paper> PaperSummery(IQueryable<Paper> sql, String tgt) //记得 加索引
        {
            return sql.Where(p => EF.Functions.ToTsVector("jiebaqry", "Summary").Matches(EF.Functions.ToTsQuery("jiebaqry", tgt)));
            //return sql + $"and (to_tsvector('jiebaqry', \"Summary\") @@ to_tsquery('jiebaqry',' {sql}') )";


        }
        public async Task<Paper> PaperTitle(string title)
        {
            var res = await _context.Papers.Select(o => o).Where(o => o.Title == title).FirstOrDefaultAsync();
            return res;
        }
        public async Task<Paper> insertPaper(Paper tgt)
        {
            var tmp = await PaperTitle(tgt.Title);
            if (tmp != null)
            {
                return null;
            }
            var result = await _context.Papers.AddAsync(tgt);
            tmp = await PaperTitle(tgt.Title);
            return tmp;
        }

        public async Task<List<Paper>> GetPapers()
        {
            return await _context.Papers.ToListAsync();
        }
    }
}
