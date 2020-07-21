using System;
using System.Linq;
using System.Linq.Expressions;

namespace Contacts.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll(bool trackChanges);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
