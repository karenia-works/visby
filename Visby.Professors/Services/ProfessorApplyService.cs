using Karenia.Visby.Professors.Models;

namespace Karenia.Visby.Professors.Services
{
    public class ProfessorApplyService
    {
        private readonly ProfessorContext _context;

        public ProfessorApplyService(ProfessorContext professorContext)
        {
            _context = professorContext;
        }
    }
}