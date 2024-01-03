
using Microsoft.EntityFrameworkCore;
using PersonalPortfolio.Data;
using PersonalPortfolio.Repository.IRepository;
using System.Linq.Expressions;

namespace PersonalPortfolio.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task<bool> DoesExist(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            return await query.AnyAsync(filter);
        }

            public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
