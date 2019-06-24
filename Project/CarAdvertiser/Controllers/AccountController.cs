using CarAdvertiser.DAL.Identity;
using CarAdvertiser.DTO;
using CarAdvertiser.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarAdvertiser.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
        {
        }
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            SignInStatus result = await RepositoryAppUserV2.LoginAsync(model.Email, model.Password, model.RememberMe);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUserV2 { UserName = model.Email, Email = model.Email };

                RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
                RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
                var result = await RepositoryAppUserV2.RegisterAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await RepositoryAppUserV2.SignInAsync(user, false, false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId < 1 || string.IsNullOrEmpty(code))
            {
                return View("Error");
            }
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            var result = await RepositoryAppUserV2.ConfirmEmailAsync(userId, code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
                RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
                var user = await RepositoryAppUserV2.FindByNameAsync(model.Email);
                if (user == null || !(await RepositoryAppUserV2.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPasswordConfirmation");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            RepositoryAppUserV2.AppUserV2Manager = HttpContext.GetOwinContext().Get<AppUserV2Manager>();
            RepositoryAppUserV2.AppV2SignInManager = HttpContext.GetOwinContext().Get<AppV2SignInManager>();
            AppUserV2 user = await RepositoryAppUserV2.FindByNameAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            IdentityResult result = await RepositoryAppUserV2.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}