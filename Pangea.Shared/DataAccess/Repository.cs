using Microsoft.EntityFrameworkCore;
using Pangea.Shared.DataAccess.Contracts;
using System.Linq.Expressions;

namespace Pangea.Shared.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Class members

        protected readonly DbContext _context;

        #endregion

        #region Constructors

        public Repository(DbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public TEntity Get<TKey>(TKey id) where TKey : struct
        {
            return _context.Set<TEntity>().Find(id)!;
        }

        public async Task<TEntity> GetAsync<TKey>(TKey id, CancellationToken cancellationToken) where TKey : struct
        {
            return (await _context.Set<TEntity>().FindAsync(id, cancellationToken))!;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().ToListAsync(cancellationToken);
        }
     
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity,bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        #endregion
    }
}
