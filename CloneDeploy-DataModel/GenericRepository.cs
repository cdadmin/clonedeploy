using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CloneDeploy_DataModel
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public string Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            return query.Count().ToString();
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            _dbSet.RemoveRange(query);
        }

        public virtual void ExecuteRawSql(string query, params object[] parameters)
        {
            _dbSet.SqlQuery(query, parameters);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> filter = null)
        {
            return _dbSet.Any(filter);
        }

        public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var includeProperty in includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query.FirstOrDefault();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity, object id)
        {
            var entityToUpdate = _dbSet.Find(id);
            if (entityToUpdate == null) return;
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        }
    }
}