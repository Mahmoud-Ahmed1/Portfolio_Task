using PersonalPortfolio.Models;

namespace PersonalPortfolio.Repository.IRepository
{
    public interface IPortFolioRepository:IRepository<PortfolioItem>
    {
        Task<PortfolioItem> Update(PortfolioItem portfolio);
    }
}
