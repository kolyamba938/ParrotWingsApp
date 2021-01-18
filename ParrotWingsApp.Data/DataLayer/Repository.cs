using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ParrotWingsApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParrotWingsApp.Data.DataLayer
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        readonly DbContext _context;
        readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public void Delete(T entity)
        {
            T existing = _context.Set<T>().Find(entity);
            if (existing != null) _context.Set<T>().Remove(existing);
        }

        public void Delete(Guid id)
        {
            T existing = _context.Set<T>().Find(id);
            if (existing != null) _context.Set<T>().Remove(existing);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public IEnumerable<T> Get()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Update(T entity)
        {
            //_context.Entry(entity).State = EntityState.Modified;
            //_context.Set<T>().Attach(entity);
            _context.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IDbContextTransaction CreateDbTransaction()
        {
            return _context.Database.BeginTransaction();
        }

    }
}
