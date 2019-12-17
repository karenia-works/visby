using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Karenia.Visby.Papers.Services
{
    public class PaperService
    {
        private readonly PaperContext _context;

        public PaperService(PaperContext paperContext)
        {
            _context = paperContext;
        }

        public async Task<Paper> GetPaper(int id)
        {
            return await _context.Papers
                .Where(p => p.PaperId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Paper>> getPaperKeyword(List<String> Keywords)
        {
            var ps = await _context.Papers.FromSqlRaw("select * from Papers where keyword <@ '{0}'", Keywords)
                .ToListAsync();
            return ps;
        }
        public List<Paper> testdata()
        {
            return _context.Papers.AsQueryable().ToList();
        }

        public async Task<List<Paper>> GetPaperAuthor(List<String> authors)
        {
            var ps = await _context.Papers.FromSqlRaw("select * from public.\"Papers\" where \"Authors\" @> array[{0}]", authors)
                .ToListAsync();
            return ps;
        }

        public async Task<List<Paper>> GetPaperLocalAuthor(List<int> Localauthers)
        {
            var ps = await _context.Papers.FromSqlRaw("select * from Papers where Localauthor <@'{0}'", Localauthers)
                .ToListAsync();
            return ps;
        }

        public async Task<List<Paper>> GetPaperSummery(String tgt) //记得 加索引
        {
            var ps = await _context.Papers
                .FromSqlRaw(
                   " select * from \"Papers\" where to_tsvector('jiebaqry', \"Summary\") @@ to_tsquery('jiebaqry', {0}) ",
                    tgt).ToListAsync();
            return ps;
        }
        public async void insertPaper(Paper tgt)
        {
            var result = await _context.Papers.AddAsync(tgt);
        }

        public async Task<List<Paper>> GetPapers()
        {
            return await _context.Papers.ToListAsync();
        }
    }
}