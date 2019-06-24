using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IAppRoleStore<T> : IRoleStore<T, int> where T : AppRole
    {
    }
}