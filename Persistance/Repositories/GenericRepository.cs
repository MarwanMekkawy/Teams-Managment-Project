using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        // getting list entities <TEntity> tracked/not tracked [paginated]
        public async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, bool Tracked = false)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            if(!Tracked) query = query.AsNoTracking();         
            return await query.OrderBy(e => e.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        // getting an entity <TEntity> by id
        public async Task<TEntity?> GetAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        // adding entity
        public void  Add(TEntity entity)
        {
            _context.Add(entity);
        }
        // updating entity
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        // hard deleting entity
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }        
        // check if entity exists
        public async Task<bool> ExistsAsync(TKey id)
        {
            return await _context.Set<TEntity>().AnyAsync(e => e.Id!.Equals(id));
        }
        // gets soft deleted entities by id
        public async Task<TEntity?> GetIncludingDeletedAsync(TKey id)
        {
            return await _context.Set<TEntity>().IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id.Equals(id));
        }
        // gets all soft deleted entities[paginated]
        public async Task<List<TEntity>> GetAllSoftDeletedAsync(int pageNumber = 1, int pageSize = 10,bool tracked = false)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Set<TEntity>().IgnoreQueryFilters().Where(e => e.IsDeleted);
            if (!tracked) query = query.AsNoTracking();
            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }        
    }
}
 