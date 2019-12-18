using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karenia.Visby.Professors.Models;
using Microsoft.EntityFrameworkCore;
using Karenia.Visby.Result;

namespace Karenia.Visby.Professors.Services
{
    public class ProfessorApplyService
    {
        private readonly ProfessorContext _context;

        public ProfessorApplyService(ProfessorContext professorContext)
        {
            _context = professorContext;
        }


        public async Task<ProfessorApply> GetApply(int id)
        {
            return await _context.ProfessorApplies
                .FirstOrDefaultAsync(pa => pa.ProfessorApplyId == id);
        }

        public async Task<ProfessorApply> GetApply(string professorName)
        {
            return await _context.ProfessorApplies
                .FirstOrDefaultAsync(pa => pa.Name == professorName);
        }

        public async Task<List<ProfessorApply>> GetAppliesByProfessorId(int id)
        {
            return await _context.ProfessorApplies
                .Where(pa => pa.ProfessorId == id)
                .ToListAsync();
        }

        public async Task<Tuple<List<ProfessorApply>, int>> GetApplies(int id, int limit, int offset)
        {
            var allResult = await GetAppliesByProfessorId(id);
            int totalCount = allResult.Count;
            if (offset > totalCount)
            {
                return new Tuple<List<ProfessorApply>, int>(null, totalCount);
            }
            limit = offset + limit > totalCount ? totalCount - offset : limit;
            var subList = allResult.GetRange(offset, limit);
            return new Tuple<List<ProfessorApply>, int>(subList, totalCount);
        }

        public async Task<Tuple<bool, Result<ProfessorApply>>> CreateApply(ProfessorApply apply)
        {
            // 传入的字段并不会全，所以要重新初始化一次
            Tuple<bool, Result<ProfessorApply>> result;

            if (_context.Professors.Any(p => p.ProfessorId == apply.ProfessorId) ||
                _context.ProfessorApplies.Any(pa =>
                    pa.Name == apply.Name &&
                    pa.ProfessorId == apply.ProfessorId &&
                    pa.ApplyState == 1))
            {
                result = new Tuple<bool, Result<ProfessorApply>>(false,
                    new Result<ProfessorApply>(400, "Already applied", null));
                return result;
            }

            // 置0再保存
            apply.ProfessorApplyId = 0;

            await _context.AddAsync(apply);
            await _context.SaveChangesAsync();

            result = new Tuple<bool, Result<ProfessorApply>>(true, new Result<ProfessorApply>(200, null, apply));
            return result;
        }

        public async void DeleteApply(ProfessorApply apply)
        {
            _context.ProfessorApplies.Remove(apply);
            await _context.SaveChangesAsync();
        }
    }
}