using System.Threading.Tasks;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CarAdvertiser.DAL.Identity
{
    public class AppRoleManager : RoleManager<AppRole, int>, IAppRoleManager<AppRole>
    {
        public AppRoleManager(IRoleStore<AppRole, int> store) : base(store)
        {
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new AppRoleStore<AppRole>(context.Get<CarAdvertiserContext>()));
        }
    }
}