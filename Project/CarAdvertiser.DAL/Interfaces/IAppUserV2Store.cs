using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IAppUserV2Store<T> : IUserStore<T, int> where T : AppUserV2
    {
    }
}