using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool Tracked = false);
        Task<IEnumerable<TResult>> GetAllSelectedAsync<TResult>(Expression<Func<TEntity, TResult>> expression, int pageNumber, int pageSize, bool Tracked = false);
        Task<TEntity?> GetAsync(TKey id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity); 
        Task<bool> ExistsAsync(TKey id);
        Task<TEntity?> GetIncludingDeletedAsync(TKey id);
        Task<List<TEntity>> GetAllSoftDeletedAsync(bool tracked = false);
    }
}
