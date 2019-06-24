using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO.BaseEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CarAdvertiser.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public ICarAdvertiserContext Context { get; set; }
        protected IDbSet<T> Entities => Context.GetDbSet<T>();

        public Repository(ICarAdvertiserContext context)
        {
            Context = context;
        }

        public T GetById(int id)
        {
            return Entities.Find(id);
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includeProps)
        {
            IQueryable<T> query = Entities.Where(x => x.Id == id).AsQueryable();
            foreach (Expression<Func<T, object>> prop in includeProps)
            {
                query = query.Include(prop);
            }

            return query.SingleOrDefault();
        }

        public IQueryable<T> Query()
        {
            return Entities.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.AsEnumerable();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProps)
        {
            IQueryable<T> query = Entities.AsQueryable();
            foreach (Expression<Func<T, object>> prop in includeProps)
            {
                query = query.Include(prop);
            }

            return query.AsEnumerable();
        }

        public virtual T Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Entities.Add(entity);
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.IsDeleted = true;
            Update(entity);
        }

        public virtual void Purge(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Entities.Remove(entity);
        }

        public virtual void Undelete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (!entity.IsDeleted) throw new ArgumentException("There is nothing to restore, as the entry has not been deleted");

            entity.IsDeleted = false;
            Update(entity);
        }

        public int Save()
        {
            return Context.SaveChanges();
        }
    }
}
