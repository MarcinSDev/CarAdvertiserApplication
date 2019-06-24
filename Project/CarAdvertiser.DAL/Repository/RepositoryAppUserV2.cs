using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DAL.Interfaces;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarAdvertiser.DAL.Repository
{
    public class RepositoryAppUserV2<T> : Repository<T>, IRepositoryAppUserV2<T> where T : AppUserV2
    {
        private bool _disposed;

        public RepositoryAppUserV2(ICarAdvertiserContext context) : base(context)
        {
        }

        public IAppUserV2Manager<T> AppUserV2Manager { get; set; }

        public IAppV2SignInManager AppV2SignInManager { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                AppUserV2Manager?.Dispose();
                AppUserV2Manager = null;
                AppV2SignInManager?.Dispose();
                AppV2SignInManager = null;
                Context?.Dispose();
                Context = null;
            }

            _disposed = true;
        }

        public override T Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            T found1 = FindByName(entity.UserName);
            if (found1 != null)
            {
                if (found1.IsDeleted)
                {
                    found1.Email = entity.Email;
                    Undelete(found1);
                    return found1;
                }
                else
                {
                    throw new ArgumentException("Username already exists in database!");
                }
            }

            T found2 = FindByEmail(entity.Email);
            if (found2 != null)
            {
                if (found2.IsDeleted)
                {
                    found2.UserName = entity.UserName;
                    Undelete(found2);
                    return found2;
                }
                else
                {
                    throw new ArgumentException("Email already exists in database!");
                }
            }

            return base.Add(entity);
        }

        public Task<IdentityResult> AddAsync(T user, string password)
        {
            return AppUserV2Manager.CreateAsync(user, password);
        }

        public void Delete(string username)
        {
            T user = FindByName(username.Trim().ToLower());
            Delete(user);
        }

        public void Purge(string username)
        {
            T user = FindByName(username.Trim().ToLower());
            Purge(user);
        }

        public Task<SignInStatus> LoginAsync(string username, string password, bool rememberMe, bool shouldLockout = false)
        {
            return AppV2SignInManager.PasswordSignInAsync(username, password, rememberMe, shouldLockout: shouldLockout);
        }

        public Task SignInAsync(AppUserV2 user, bool isPersistent, bool rememberBrowser)
        {
            return AppV2SignInManager.SignInAsync(user, isPersistent, rememberBrowser);
        }

        public Task<IdentityResult> RegisterAsync(T user, string password)
        {
            return AppUserV2Manager.CreateAsync(user, password);
        }

        public Task<IdentityResult> ConfirmEmailAsync(int userId, string code)
        {
            return AppUserV2Manager.ConfirmEmailAsync(userId, code);
        }

        public Task<T> FindByIdAsync(int userId)
        {
            return AppUserV2Manager.FindByIdAsync(userId);
        }

        public T FindById(int userId)
        {
            return GetById(userId);
        }

        public Task<T> FindByNameAsync(string username)
        {
            return AppUserV2Manager.FindByNameAsync(username);
        }

        public T FindByName(string username)
        {
            return GetAll().FirstOrDefault(x => x.UserName.Equals(username));
        }

        public T FindByEmail(string email)
        {
            return GetAll().FirstOrDefault(x => x.Email.Equals(email));
        }

        public Task<bool> IsEmailConfirmedAsync(int userId)
        {
            return AppUserV2Manager.IsEmailConfirmedAsync(userId);
        }

        public Task<IdentityResult> ResetPasswordAsync(int userId, string code, string password)
        {
            return AppUserV2Manager.ResetPasswordAsync(userId, code, password);
        }

        public Task<int> GetVerifiedUserIdAsync()
        {
            return AppV2SignInManager.GetVerifiedUserIdAsync();
        }

        public Task<IList<string>> GetValidTwoFactorProvidersAsync(int userId)
        {
            return AppUserV2Manager.GetValidTwoFactorProvidersAsync(userId);
        }

        public Task<bool> SendTwoFactorCodeAsync(string selectedProvider)
        {
            return AppV2SignInManager.SendTwoFactorCodeAsync(selectedProvider);
        }

        public Task<string> GenerateChangePhoneNumberTokenAsync(int userId, string phoneNumber)
        {
            return AppUserV2Manager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);
        }

        public Task<IdentityResult> SetTwoFactorEnabledAsync(int userId, bool enabled)
        {
            return AppUserV2Manager.SetTwoFactorEnabledAsync(userId, enabled);
        }

        public Task<IdentityResult> ChangePhoneNumberAsync(int userId, string phoneNumber, string code)
        {
            return AppUserV2Manager.ChangePhoneNumberAsync(userId, phoneNumber, code);
        }

        public Task<IdentityResult> SetPhoneNumberAsync(int userId, string phoneNumber)
        {
            return AppUserV2Manager.SetPhoneNumberAsync(userId, phoneNumber);
        }

        public Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            return AppUserV2Manager.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        public Task<IdentityResult> AddPasswordAsync(int userId, string newPassword)
        {
            return AppUserV2Manager.AddPasswordAsync(userId, newPassword);
        }
    }
}