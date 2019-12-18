using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karenia.Visby.Professors.Models;
using Karenia.Visby.Result;
using Microsoft.EntityFrameworkCore;

namespace Karenia.Visby.Professors.Services
{
    public class ProfessorService
    {
        private readonly ProfessorContext _context;

        public ProfessorService(ProfessorContext professorContext)
        {
            _context = professorContext;
        }

        public async Task<Professor> GetProfessor(int id)
        {
            return await _context.Professors
                .FirstOrDefaultAsync(p => p.ProfessorId == id || p.UserId == id);
        }

        public async Task<Professor> GetProfessor(string professorName)
        {
            return await _context.Professors
                .FirstOrDefaultAsync(p => p.Name == professorName);
        }

        public async Task<List<Professor>> GetProfessorsByInstitution(string institutionKeyword)
        {
            return await _context.Professors
                .Where(p => p.Institution.Contains(institutionKeyword))
                .ToListAsync();
        }

        public async Task<Tuple<List<Professor>, int, bool>> GetProfessors(string keyword, int limit, int offset)
        {
            var allResult = await _context.Professors
                .Where(p => p.Institution.Contains(keyword) || p.Name.Contains(keyword))
                .ToListAsync();
            var totalCount = allResult.Count;
            bool overflow = offset > totalCount;
            if (overflow)
            {
                return new Tuple<List<Professor>, int, bool>(null, totalCount, overflow);
            }

            limit = offset + limit > totalCount ? totalCount - offset : limit;
            var subList = allResult.GetRange(offset, limit);
            return new Tuple<List<Professor>, int, bool>(subList, totalCount, false);
        }

        public async Task<Tuple<bool, Result<Professor>>> CreateProfessor(Professor professor)
        {
            // 传入的字段并不会全，所以要重新初始化一次
            Tuple<bool, Result<Professor>> result;

            if (_context.Professors.Any(p => p.Name == professor.Name && p.Institution == professor.Institution))
            {
                result = new Tuple<bool, Result<Professor>>(false,
                    new Result<Professor>(400, "Professor already exists", null));
                return result;
            }

            // 置0再保存
            professor.ProfessorId = 0;

            await _context.AddAsync(professor);
            await _context.SaveChangesAsync();

            result = new Tuple<bool, Result<Professor>>(true, new Result<Professor>(200, null, professor));
            return result;
        }

        public async void DeleteProfessor(Professor professor)
        {
            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();
        }
    }
}