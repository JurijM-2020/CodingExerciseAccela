using Contacts.Interfaces;
using Contacts.Model.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Contacts.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly DbSet<T> _table;
        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _table = _repositoryContext.Set<T>();
        }

        public void Create(T entity) => _table.Add(entity);
        

        public void Delete(T entity)
        {
            if (!_table.Local.Contains(entity))
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);

        }

        public void Delete(int id)
        {
            T existing = _table.Find(id);
            _table.Remove(existing);
        }

        public IQueryable<T> GetAll(bool trackChanges) =>
            !trackChanges ? _table.AsNoTracking() : _table;

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? _table.Where(expression).AsNoTracking() : _table.Where(expression);


        public void Update(T entity)
        {
            if (!_table.Local.Contains(entity))
            {
                _table.Attach(entity);
                _repositoryContext.Entry(entity).State = EntityState.Modified;
            }

        }
        
    }
}
