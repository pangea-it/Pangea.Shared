using System.Linq.Expressions;

namespace Pangea.Shared.DataAccess.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get<TKey>(TKey id) where TKey : struct;
        Task<TEntity> GetAsync<TKey>(TKey id, CancellationToken cancellationToken) where TKey : struct;
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}