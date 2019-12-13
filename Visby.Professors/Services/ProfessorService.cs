using Karenia.Visby.Professors.Models;

namespace Karenia.Visby.Professors.Services
{
    public class ProfessorService
    {
        private readonly ProfessorContext _context;

        public ProfessorService(ProfessorContext professorContext)
        {
            _context = professorContext;
        }
    }
}