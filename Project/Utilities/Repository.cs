using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Project.Models;

namespace Project.Utilities
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            this._entities = context.Set<T>();
        }

        public IEnumerable<T> GetAllItems(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> items = this._entities;

            if (include != null)
            {
                items = include(items);
            }

            return items.AsEnumerable();
        }

        public T GetItemById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var item = this._entities.Where(e => e.Id == id);

            if (item != null)
            {
                if (include != null)
                {
                    item = include(item);
                }

                return item.FirstOrDefault();
            }

            return null;
        }

        public void Create(T entity)
        {
            this._entities.Add(entity);
            this._context.SaveChanges();
        }

        public void Update(T entity)
        {
            this._entities.Update(entity);
            this._context.SaveChanges();
        }

        public void Delete(T entity)
        {
            this._entities.Remove(entity);
            this._context.SaveChanges();
        }
    }
}