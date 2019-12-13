using Karenia.Visby.Professor.Models;

namespace Karenia.Visby.Professor.Services
{
    public class UserProfileService
    {
        private readonly ProfessorContext _context;

        public UserProfileService(ProfessorContext professorContext)
        {
            _context = professorContext;
        }
    }
}