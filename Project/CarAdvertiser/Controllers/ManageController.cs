using CarAdvertiser.DAL.Identity;
using CarAdvertiser.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarAdvertiser.DTO;

namespace CarAdvertiser.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {

        public ManageController()
        {
        }

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();

            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var code = await RepositoryAppUserV2.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), model.Number);

            if (RepositoryAppUserV2.AppUserV2Manager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await RepositoryAppUserV2.AppUserV2Manager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            await RepositoryAppUserV2.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user != null)
            {
                await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            await RepositoryAppUserV2.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user != null)
            {
                await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var code = await RepositoryAppUserV2.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);

            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var result = await RepositoryAppUserV2.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);

            if (result.Succeeded)
            {
                var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var result = await RepositoryAppUserV2.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);

            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var result = await RepositoryAppUserV2.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
                RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
                var result = await RepositoryAppUserV2.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);

                if (result.Succeeded)
                {
                    var user = await RepositoryAppUserV2.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await RepositoryAppUserV2.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RepositoryAppUserV2?.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = RepositoryAppUserV2.AppUserV2Manager.FindById(User.Identity.GetUserId<int>());

            //var user = RepositoryAppUserV2.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        private bool HasPhoneNumber()
        {
            var user = RepositoryAppUserV2.FindById(User.Identity.GetUserId<int>());
            return user?.PhoneNumber != null;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}