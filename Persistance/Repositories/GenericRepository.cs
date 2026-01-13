using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        // getting list of all entities <TEntity> as tracked/not tracked
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool Tracked = false)
        {
            if (Tracked) return await _context.Set<TEntity>().ToListAsync();
            else         return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        // getting list of certain properties of all entities <TEntity> tracked/not tracked
        public async Task<IEnumerable<TResult>> GetAllSelectedAsync<TResult>(Expression<Func<TEntity, TResult>> expression, bool Tracked = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if(!Tracked) query = query.AsNoTracking();         
            return await query.Select(expression).ToListAsync();
        }
        // getting an entity <TEntity> by id
        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        // adding entity
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
        // updating entity
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        //hard deleting entity
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }
    }
}
