using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CloneDeploy_DataModel
{
    public interface IGenericRepository<TEntity>
    {
        void ExecuteRawSql(string query, params object[] parameters);

        List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");


        TEntity GetById(object id);


        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        string Count(Expression<Func<TEntity, bool>> filter = null);

        bool Exists(Expression<Func<TEntity, bool>> filter = null);

        void Insert(TEntity entity);

        void Update(TEntity entity, object id);

        void Delete(object id);

        void DeleteRange(Expression<Func<TEntity, bool>> filter = null);
    }
}