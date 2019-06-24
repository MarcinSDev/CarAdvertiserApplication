using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CarAdvertiser.DTO.Interfaces;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IRepository<T> where T : IBaseEntity
    {
        ICarAdvertiserContext Context { get; set; }

        /// <summary>
        /// For all the entities except AppUserV2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);
        T GetById(int id, params Expression<Func<T, object>>[] includeProps);
        IQueryable<T> Query();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProps);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Purge(T entity);
        void Undelete(T entity);
        int Save();
    }
}
