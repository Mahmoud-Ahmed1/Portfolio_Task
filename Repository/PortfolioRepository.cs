using PersonalPortfolio.Data;
using PersonalPortfolio.Models;
using PersonalPortfolio.Repository.IRepository;

namespace PersonalPortfolio.Repository
{
    public class PortfolioRepository:Repository<PortfolioItem>,IPortFolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PortfolioItem> Update(PortfolioItem entity)
        {
            _context.PortfolioItems.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
