using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarAdvertiser.DAL.Identity
{
    public class AppUserV2Store<T> :
        IAppUserV2Store<T>,
        IUserPasswordStore<T, int>,
        IUserEmailStore<T, int>,
        IUserLockoutStore<T, int>,
        IUserTwoFactorStore<T, int>,
        IUserLoginStore<T, int>
        where T : AppUserV2
    {
        private readonly UserStore<AppIdentityUser, IdentityRole<int, IdentityUserRole<int>>, int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>> _userStore;
        private readonly CarAdvertiserContext _context;
        private bool _disposed;

        public AppUserV2Store()
        {
            _context = new CarAdvertiserContext();
            _userStore = new UserStore<AppIdentityUser, IdentityRole<int, IdentityUserRole<int>>, int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>(_context);
        }

        public AppUserV2Store(ICarAdvertiserContext context)
        {
            _context = context as CarAdvertiserContext;
            _userStore = new UserStore<AppIdentityUser, IdentityRole<int, IdentityUserRole<int>>, int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _userStore?.Dispose();
                _context?.Dispose();
                _disposed = true;
            }
        }

        public Task CreateAsync(T user)
        {
            _context.GetDbSet<T>().Add(user);
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T user)
        {
            _context.GetDbSet<T>().Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(T user)
        {
            _context.GetDbSet<T>().Remove(user);
            _context.Configuration.ValidateOnSaveEnabled = false;
            return _context.SaveChangesAsync();
        }

        public Task<T> FindByIdAsync(int userId)
        {
            return _context.GetDbSet<T>().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<T> FindByNameAsync(string userName)
        {
            return _context.GetDbSet<T>().FirstOrDefaultAsync(x => x.UserName.Equals(userName));
        }

        public Task SetPasswordHashAsync(T user, string passwordHash)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            Task task = _userStore.SetPasswordHashAsync(identityUser, passwordHash);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<string> GetPasswordHashAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            Task<string> task = _userStore.GetPasswordHashAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<bool> HasPasswordAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            Task<bool> task = _userStore.HasPasswordAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        private static void SetApplicationUser(T user, AppIdentityUser identityUser)
        {
            user.PasswordHash = identityUser.PasswordHash;
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName;
        }

        private static AppIdentityUser ToIdentityUser(T user)
        {

            return new AppIdentityUser
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task SetEmailAsync(T user, string email)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            await _userStore.SetEmailAsync(identityUser, email);
        }

        public async Task<string> GetEmailAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return await _userStore.GetEmailAsync(identityUser);
        }

        public async Task<bool> GetEmailConfirmedAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return await _userStore.GetEmailConfirmedAsync(identityUser);
        }

        public async Task SetEmailConfirmedAsync(T user, bool confirmed)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            await _userStore.SetEmailConfirmedAsync(identityUser, confirmed);
        }

        public async Task<T> FindByEmailAsync(string email)
        {
            return await _context.GetDbSet<T>().FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.GetLockoutEndDateAsync(identityUser);
        }

        public Task SetLockoutEndDateAsync(T user, DateTimeOffset lockoutEnd)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.SetLockoutEndDateAsync(identityUser, lockoutEnd);
        }

        public Task<int> IncrementAccessFailedCountAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.IncrementAccessFailedCountAsync(identityUser);
        }

        public Task ResetAccessFailedCountAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.ResetAccessFailedCountAsync(identityUser);
        }

        public Task<int> GetAccessFailedCountAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.GetAccessFailedCountAsync(identityUser);
        }

        public Task<bool> GetLockoutEnabledAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.GetLockoutEnabledAsync(identityUser);
        }

        public Task SetLockoutEnabledAsync(T user, bool enabled)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.SetLockoutEnabledAsync(identityUser, enabled);
        }

        public Task SetTwoFactorEnabledAsync(T user, bool enabled)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.SetTwoFactorEnabledAsync(identityUser, enabled);
        }

        public Task<bool> GetTwoFactorEnabledAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.GetTwoFactorEnabledAsync(identityUser);
        }

        public Task AddLoginAsync(T user, UserLoginInfo login)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.AddLoginAsync(identityUser, login);
        }

        public Task RemoveLoginAsync(T user, UserLoginInfo login)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            return _userStore.RemoveLoginAsync(identityUser, login);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(T user)
        {
            AppIdentityUser identityUser = ToIdentityUser(user);
            IList<UserLoginInfo> result = _userStore.GetLoginsAsync(identityUser).Result;
            return _userStore.GetLoginsAsync(identityUser);
        }

        public Task<T> FindAsync(UserLoginInfo login)
        {
            return null;
        }
    }
}