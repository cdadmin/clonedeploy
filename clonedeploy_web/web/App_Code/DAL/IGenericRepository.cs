using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public interface IGenericRepository<TEntity>
    {
        List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetById(object id);


        TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);


        void Insert(TEntity entity);

        void Update(TEntity entity, object id);

        void Delete(object id);
    }
}