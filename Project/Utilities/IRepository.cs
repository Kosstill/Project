using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Project.Models;

namespace Project.Utilities
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAllItems(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        T GetItemById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}