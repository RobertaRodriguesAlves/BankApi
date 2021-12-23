using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected BankDbContext context;
        public BaseRepository(BankDbContext context) => this.context = context;

        public void Create(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            this.context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> FindAll()
        {
            return this.context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return this.context.Set<TEntity>().Where(expression).AsNoTracking();
        }

        public void Update(TEntity entity)
        {
            this.context.Set<TEntity>().Update(entity);
        }
    }
}
