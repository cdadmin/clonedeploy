using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DAL;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private DbContext _context;
    private DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        this._context = context;
        this._dbSet = context.Set<TEntity>();
    }

    public string Count()
    {
        return _dbSet.Count().ToString();
    }

    public virtual List<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = orderBy(query);

        return query.ToList();
    }

    public virtual TEntity GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (Expression<Func<TEntity, object>> include in includes)
            query = query.Include(include);

        return query.FirstOrDefault(filter);
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> filter = null)
    {
        return _dbSet.Any(filter);
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Update(TEntity entity, object id)
    {
        TEntity entityToUpdate = _dbSet.Find(id);
        if (entityToUpdate == null) return;
        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        //_context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(object id)
    {
        TEntity entityToDelete = _dbSet.Find(id);
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }
}