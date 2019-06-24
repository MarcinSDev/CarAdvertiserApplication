using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CarAdvertiser.DTO.Interfaces;

namespace CarAdvertiser.BLL.Interfaces
{
    public interface IService<T> where T : IBaseEntity
    {
        T FindById(int id);
        T Create(T entity);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProps);
        IEnumerable<T> GetAllNotDeleted();
        IEnumerable<T> GetAllNotDeleted(params Expression<Func<T, object>>[] includeProps);
        IEnumerable<T> GetAllDeleted();
        IEnumerable<T> GetAllDeleted(params Expression<Func<T, object>>[] includeProps);
        T Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        void Undelete(T entity);
        void Undelete(int id);
        void Purge(T entity);
        void Purge(int id);
        void PurgeAll();
        int Save();
    }
}
