using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IRepositoryAppUserV2<T> : IRepository<T>, IDisposable where T : AppUserV2
    {
        IAppUserV2Manager<T> AppUserV2Manager { get; set; }
        IAppV2SignInManager AppV2SignInManager { get; set; }
        Task<IdentityResult> AddAsync(T user, string password);
        void Delete(string username);
        void Purge(string username);
        Task<SignInStatus> LoginAsync(string username, string password, bool rememberMe, bool shouldLockout = false);
        Task SignInAsync(AppUserV2 user, bool isPersistent, bool rememberBrowser);
        Task<IdentityResult> RegisterAsync(T user, string password);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);
        Task<T> FindByIdAsync(int userId);
        T FindById(int userId);
        Task<T> FindByNameAsync(string username);
        T FindByName(string username);
        T FindByEmail(string email);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<IdentityResult> ResetPasswordAsync(int userId, string code, string password);
        Task<int> GetVerifiedUserIdAsync();
        Task<IList<string>> GetValidTwoFactorProvidersAsync(int userId);
        Task<bool> SendTwoFactorCodeAsync(string selectedProvider);
        Task<string> GenerateChangePhoneNumberTokenAsync(int userId, string phoneNumber);
        Task<IdentityResult> SetTwoFactorEnabledAsync(int userId, bool enabled);
        Task<IdentityResult> ChangePhoneNumberAsync(int userId, string phoneNumber, string code);
        Task<IdentityResult> SetPhoneNumberAsync(int userId, string phoneNumber);
        Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(int userId, string newPassword);
    }
}