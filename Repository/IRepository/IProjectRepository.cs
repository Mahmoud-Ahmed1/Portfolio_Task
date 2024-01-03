using PersonalPortfolio.Models;

namespace PersonalPortfolio.Repository.IRepository
{
    public interface IProjectRepository:IRepository<Project>
    {
        Task<Project> Update(Project project);

    }
}
