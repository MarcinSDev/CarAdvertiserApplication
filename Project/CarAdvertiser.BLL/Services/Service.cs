using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.BLL.Interfaces;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO.Interfaces;

namespace CarAdvertiser.BLL.Services
{
    public class Service<T> : IService<T> where T : IAuditableEntity
    {
        protected readonly IUnitOfWork Uow;
        protected readonly IRepository<T> Repository;

        public Service(IRepository<T> repository, IUnitOfWork uow)
        {
            Repository = repository;
            Uow = uow;
        }

        public T Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Repository.Add(entity);

            return entity;

        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(int id)
        {
            T entity = Repository.GetById(id);

            Delete(entity);
        }

        public T FindById(int id)
        {
            return Repository.GetById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps);
        }

        public virtual IEnumerable<T> GetAllDeleted()
        {
            return Repository.GetAll().Where(x => x.IsDeleted);
        }

        public virtual IEnumerable<T> GetAllDeleted(params Expression<Func<T, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(x => x.IsDeleted);
        }

        public virtual IEnumerable<T> GetAllNotDeleted()
        {
            return Repository.GetAll().Where(x => !x.IsDeleted);
        }

        public virtual IEnumerable<T> GetAllNotDeleted(params Expression<Func<T, object>>[] includeProps)
        {
            return Repository.GetAll(includeProps).Where(x => !x.IsDeleted);
        }

        public void Purge(T entity)
        {
            Repository.Purge(entity);
        }

        public void Purge(int id)
        {
            T entity = Repository.GetById(id);

            Purge(entity);
        }

        public void PurgeAll()
        {
            foreach (T item in GetAll())
            {
                Purge(item);
            }
        }

        public int Save()
        {
            return Uow.Commit();
        }

        public void Undelete(T entity)
        {
            Repository.Undelete(entity);
        }

        public void Undelete(int id)
        {
            T entity = Repository.GetById(id);

            Undelete(entity);
        }

        public T Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Repository.Update(entity);
            return entity;
        }
    }
}
