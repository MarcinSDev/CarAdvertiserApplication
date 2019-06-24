using System;
using System.Collections.Generic;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Interfaces;

namespace CarAdvertiser.DAL.Identity
{
    public class AppV2SignInManager : SignInManager<AppUserV2, int>, IAppV2SignInManager
    {
        public AppV2SignInManager(UserManager<AppUserV2, int> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUserV2 user)
        {         
            AppUserV2Manager userManager = (AppUserV2Manager)UserManager;
            return userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static AppV2SignInManager Create(IdentityFactoryOptions<AppV2SignInManager> option, IOwinContext context)
        {
            return new AppV2SignInManager(context.GetUserManager<AppUserV2Manager>(), context.Authentication);
        }

        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            
            return PasswordSignInAsyncCustom(userName, password, isPersistent, shouldLockout);
        }

        private async Task<SignInStatus> PasswordSignInAsyncCustom(string userName, string password, bool isPersistent,
            bool shouldLockout)
        {
            SignInStatus signInStatu;
            if (this.UserManager != null)
            {
                Task<AppUserV2> userAwaiter = ((AppUserV2Manager)UserManager).FindByEmailAsync(userName);

                AppUserV2 tUser = await userAwaiter;
                if (tUser != null)
                {
                    Task<bool> cultureAwaiter1 = this.UserManager.IsLockedOutAsync(tUser.Id);
                    if (!await cultureAwaiter1)
                    {
                        Task<bool> cultureAwaiter2 = this.UserManager.CheckPasswordAsync(tUser, password);
                        if (!await cultureAwaiter2)
                        {
                            if (shouldLockout)
                            {
                                Task<IdentityResult> cultureAwaiter3 = this.UserManager.AccessFailedAsync(tUser.Id);
                                await cultureAwaiter3;
                                Task<bool> cultureAwaiter4 = this.UserManager.IsLockedOutAsync(tUser.Id);
                                if (await cultureAwaiter4)
                                {
                                    signInStatu = SignInStatus.LockedOut;
                                    return signInStatu;
                                }
                            }
                            signInStatu = SignInStatus.Failure;
                        }
                        else
                        {
                            Task<IdentityResult> cultureAwaiter5 = this.UserManager.ResetAccessFailedCountAsync(tUser.Id);
                            await cultureAwaiter5;
                            await SignInAsync(tUser, isPersistent, false);
                            signInStatu = SignInStatus.Success;
                        }
                    }
                    else
                    {
                        signInStatu = SignInStatus.LockedOut;
                    }
                }
                else
                {
                    signInStatu = SignInStatus.Failure;
                }
            }
            else
            {
                signInStatu = SignInStatus.Failure;
            }
            return signInStatu;
        }

        private async Task<SignInStatus> SignInOrTwoFactor(AppUserV2 user, bool isPersistent)
        {
            SignInStatus signInStatu;
            string str = Convert.ToString(user.Id);
            Task<bool> cultureAwaiter = this.UserManager.GetTwoFactorEnabledAsync(user.Id);
            if (await cultureAwaiter)
            {
                Task<IList<string>> providerAwaiter = this.UserManager.GetValidTwoFactorProvidersAsync(user.Id);
                IList<string> listProviders = await providerAwaiter;
                if (listProviders.Count > 0)
                {
                    Task<bool> cultureAwaiter2 = AuthenticationManagerExtensions.TwoFactorBrowserRememberedAsync(this.AuthenticationManager, str);
                    if (!await cultureAwaiter2)
                    {
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity("TwoFactorCookie");
                        claimsIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", str));
                        this.AuthenticationManager.SignIn(new ClaimsIdentity[] { claimsIdentity });
                        signInStatu = SignInStatus.RequiresVerification;
                        return signInStatu;
                    }
                }
            }
            Task cultureAwaiter3 = this.SignInAsync(user, isPersistent, false);
            await cultureAwaiter3;
            signInStatu = SignInStatus.Success;
            return signInStatu;
        }
    }
}