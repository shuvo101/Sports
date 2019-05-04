using Microsoft.EntityFrameworkCore;
using Sports.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sports.API.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        public IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;
            if (query != null)
            {
                if (query.ToList().Count != 0)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty);
                    }
                }

            }

            return query;
        }

        public virtual IEnumerable<TEntity> GetAllSorted<TType>(Expression<Func<TEntity, TType>> sortCondition, bool sortDesc)
        {
            if (sortDesc)
            {
                return DbSet.OrderByDescending(sortCondition);
            }

            return DbSet.OrderBy(sortCondition);
        }

        public virtual TEntity GetById(object id)
        {
            var xxx = DbSet.Find(id);
            return xxx;
        }

        public virtual TEntity GetByIdIncluding(object id, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var model = DbSet.Find(id);

            if (model != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    Context.Entry(model).Reference(includeProperty).Load();
                }
            }

            return model;
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void AddAsync(TEntity entity)
        {
            DbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAllSorted<TType>(Expression<Func<TEntity, TType>> sortCondition, bool sortDesc);
        TEntity GetById(object id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Save();
    }
}
