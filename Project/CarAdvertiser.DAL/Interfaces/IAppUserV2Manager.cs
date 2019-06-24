using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.DAL.Interfaces
{
    public interface IAppUserV2Manager<T> : IDisposable where T : AppUserV2
    {
        IIdentityMessageService SmsService { get; set; }
        IIdentityMessageService EmailService { get; set; }
        Task<IdentityResult> CreateAsync(T user, string password);
        Task<IdentityResult> ConfirmEmailAsync(int userId, string code);
        Task<ClaimsIdentity> GenerateUserIdentityAsync(T user);
        Task<T> FindByIdAsync(int userId);
        T FindById(int userId);
        Task<T> FindByNameAsync(string username);
        T FindByName(string username);
        T FindByEmail(string email);
        Task<bool> IsEmailConfirmedAsync(int userId);
        Task<IdentityResult> ResetPasswordAsync(int userId, string code, string password);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(int userId);
        Task<string> GenerateChangePhoneNumberTokenAsync(int userId, string phoneNumber);
        Task<IdentityResult> SetTwoFactorEnabledAsync(int userId, bool enabled);
        Task<IdentityResult> ChangePhoneNumberAsync(int userId, string phoneNumber, string code);
        Task<IdentityResult> SetPhoneNumberAsync(int userId, string phoneNumber);
        Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(int userId, string newPassword);
    }
}