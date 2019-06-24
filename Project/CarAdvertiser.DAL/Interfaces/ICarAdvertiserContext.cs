using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CarAdvertiser.DTO.BaseEntities;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface ICarAdvertiserContext:IDisposable
    {
        IDbSet<T> GetDbSet<T>() where T : BaseEntity;
        DbEntityEntry<T> Entry<T>(T entity) where T : BaseEntity;
        int SaveChanges();
    }
}