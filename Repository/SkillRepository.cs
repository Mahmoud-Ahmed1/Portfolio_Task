using PersonalPortfolio.Data;
using PersonalPortfolio.Models;
using PersonalPortfolio.Repository.IRepository;

namespace PersonalPortfolio.Repository
{
    public class SkillRepository:Repository<Skill>,ISkillRepository
    {
        private readonly ApplicationDbContext _context;
        public SkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Skill> Update(Skill entity)
        {
            _context.Skills.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
