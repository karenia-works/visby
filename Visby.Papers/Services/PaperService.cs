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

        public async Task<List<Paper>> GetPaperByKeyword(List<String> keywords)
        {
            return await _context.Papers
                .Where(p => p.Keywords.Except(keywords).Any())
                .ToListAsync();
        }
    }
}