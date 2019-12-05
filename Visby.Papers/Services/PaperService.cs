using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
    }
}