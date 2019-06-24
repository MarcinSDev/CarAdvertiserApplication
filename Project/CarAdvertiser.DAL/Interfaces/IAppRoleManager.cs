using CarAdvertiser.DTO;
using System;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IAppRoleManager<T> : IDisposable where T : AppRole
    {
    }
}