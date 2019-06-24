using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarAdvertiser.DAL.Identity
{
    public class AppUserV2Manager : UserManager<AppUserV2, int>, IAppUserV2Manager<AppUserV2>
    {
        public AppUserV2Manager(IAppUserV2Store<AppUserV2> store) : base(store)
        {
        }

        public static AppUserV2Manager Create(IdentityFactoryOptions<AppUserV2Manager> options, IOwinContext context)
        {
            AppUserV2Manager manager = new AppUserV2Manager(new AppUserV2Store<AppUserV2>(context.Get<CarAdvertiserContext>()));

            manager.UserValidator = new UserValidator<AppUserV2, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<AppUserV2, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<AppUserV2, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailV2Service();
            manager.SmsService = new SmsService();
            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AppUserV2, int>(dataProtectionProvider.Create("ASP.NET Custom Identity"));
            }

            return manager;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserV2 user)
        {
            return CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public AppUserV2 FindById(int userId)
        {
            return FindByIdAsync(userId).Result;
        }

        public AppUserV2 FindByName(string username)
        {
            return FindByNameAsync(username).Result;
        }

        public AppUserV2 FindByEmail(string email)
        {
            return FindByEmailAsync(email).Result;
        }
    }
}
