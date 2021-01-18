using ParrotWingsApp.Data.Model;
using System;
using System.Collections.Generic;

namespace ParrotWingsApp.Data.DataLayer
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void Add(IEnumerable<T> entities);


        void Delete(T entity);
        void Delete(Guid id);
        void Delete(IEnumerable<T> entities);


        void Update(T entity);
        void Update(IEnumerable<T> entities);

        void SaveChanges();
    }
}
