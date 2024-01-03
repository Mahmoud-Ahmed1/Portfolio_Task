using PersonalPortfolio.Models;

namespace PersonalPortfolio.Repository.IRepository
{
    public interface ISkillRepository:IRepository<Skill>
    {
        Task<Skill> Update(Skill skill);
    }
}
