using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity.Owin;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IAppV2SignInManager : IDisposable//needed for repository pattern
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);
        Task SignInAsync(AppUserV2 user, bool isPersistent, bool rememberBrowser);
        Task<int> GetVerifiedUserIdAsync();
        Task<bool> SendTwoFactorCodeAsync(string selectedProvider);

    }
}
