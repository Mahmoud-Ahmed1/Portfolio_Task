using PersonalPortfolio.Data;
using PersonalPortfolio.Models;
using PersonalPortfolio.Repository.IRepository;

namespace PersonalPortfolio.Repository
{
    public class ProjectRepository:Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Project> Update(Project entity)
        {
            _context.Projects.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
