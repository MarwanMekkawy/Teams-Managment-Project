using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool Tracked = false)
        {
            if (Tracked) return await _context.Set<TEntity>().ToListAsync();
            else         return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
    }
}
