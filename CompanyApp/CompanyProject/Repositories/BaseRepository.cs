
using CompanyProject.Data;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        protected readonly CompanyProjectDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(CompanyProjectDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existing = await _dbSet.FindAsync(id);
            if(existing is not null)
            {
                _dbSet.Remove(existing);
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _dbSet.CountAsync();
            return count;
        }

        public virtual void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
