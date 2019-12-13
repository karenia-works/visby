using Karenia.Visby.Professor.Models;

namespace Karenia.Visby.Professor.Services
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