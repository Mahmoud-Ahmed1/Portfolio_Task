using System.Linq.Expressions;

namespace PersonalPortfolio.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Create(T entity);
        Task Remove(T entity);
        Task Save();
        Task<bool> DoesExist(Expression<Func<T, bool>> filter = null);
    }
}
