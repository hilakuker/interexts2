using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Interext.Models;
using System.Web.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Interext.OtherCalsses;
using TestApp.Models;

namespace Interext.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            var userValidator = UserManager.UserValidator as UserValidator<ApplicationUser>;
            userValidator.AllowOnlyAlphanumericUserNames = false;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        public async Task<ActionResult> ShowProfile(string returnUrl)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            ProfileViewModel profile = new ProfileViewModel();
            //profile.UserName = user.UserName;
            profile.FirstName = user.FirstName;
            profile.LastName = user.LastName;
            profile.Email = user.Email;
            if (user.Gender == "F")
                profile.Gender = "Female";
            else
                profile.Gender = "Male";
            //add age
            profile.Interests = null;
            profile.Events = null;
            profile.ImageUrl = user.ImageUrl;
            profile.BirthDate = getBirthdateAndAge(user.BirthDate);
            profile.Address = user.HomeAddress;
            return View(profile);
        }

        private string getBirthdateAndAge(DateTime? i_BirthDate)
        {
            string finalResult = "";
            int birthDateYear;
            int todayYear = DateTime.Today.Year;
            if(i_BirthDate.HasValue == true)
            {
                string birthDateString = i_BirthDate.Value.ToShortDateString();
                birthDateYear = i_BirthDate.Value.Year;
                 int age = todayYear - birthDateYear - 1;
                 finalResult = string.Format("{0} ({1} year old)", birthDateString, age.ToString());
            }
            return finalResult;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(GenerateUserName(model.Email), model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChooseRegisterProvider
        [AllowAnonymous]
        public ActionResult ChooseRegisterProvider()
        {
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase ImageUrl)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() {
                    UserName = GenerateUserName(model.Email),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    BirthDate = model.BirthDate.Date,
                    HomeAddress = model.Address
                };
                uploadAndSetImage(ref user, ImageUrl);
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

    

        private void uploadAndSetImage(ref ApplicationUser user, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                string pathForToSave = Path.Combine(Server.MapPath("~/Content/images"), user.Id);
                string fileName = Path.GetFileName(ImageUrl.FileName);
                string pathForPicture = string.Format(@"/Content/images/{0}/{1}", user.Id, fileName);
                user.ImageUrl = pathForPicture;
                ImageSaver.SaveImage(ImageUrl, pathForToSave, fileName);
                Server.MapPath(pathForPicture);
            }
        }

        public string GenerateUserName(string email)
        {
            return email.Replace("@", "").Replace(".", "").Replace("-", "");
        }
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager_GetExternalLoginInfoAsync_Workaround();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                Task<ClaimsIdentity> externalIdentity = getExternalIdentity();
                var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email});
            }
            //var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);
            //if (result == null || result.Identity == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
            //if (idClaim == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //var login = new UserLoginInfo(idClaim.Issuer, idClaim.Value);
            //var name = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", "");

            //// Sign in the user with this external login provider if the user already has a login
            //var user = await UserManager.FindAsync(login);
            //if (user != null)
            //{
            //    await SignInAsync(user, isPersistent: false);
            //    return RedirectToLocal(returnUrl);
            //}
            //else
            //{
            //    // If the user does not have an account, then prompt the user to create an account
            //    ViewBag.ReturnUrl = returnUrl;
            //    ViewBag.LoginProvider = login.LoginProvider;
            //    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = name });
            //}
        }

        public ActionResult SetProfilePictureInLayout(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public async Task<ActionResult> SetProfilePictureInLayout(ImageModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            model.ImageUrl = user.ImageUrl;
            }
            return View(model);

        }

        private async Task<ExternalLoginInfo> AuthenticationManager_GetExternalLoginInfoAsync_Workaround()
        {
            ExternalLoginInfo loginInfo = null;
            var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (result != null && result.Identity != null)
            {
                var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim != null)
                {
                    loginInfo = new ExternalLoginInfo()
                    {
                        DefaultUserName = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", ""),
                        Login = new UserLoginInfo(idClaim.Issuer, idClaim.Value)
                    };
                }
            }
            return loginInfo;
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager_GetExternalLoginInfoAsync_Workaround();
                ApplicationUser user;
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                string loginProviderLowerCase = info.Login.LoginProvider.ToLower();
                if (loginProviderLowerCase == "facebook")
                {
                    user = createUserFromFacebookInfo(model);
                }
                else
                {
                    user = new ApplicationUser()
                    {
                        UserName = GenerateUserName(model.Email),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Gender = model.Gender
                    };
                }
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private ApplicationUser createUserFromFacebookInfo(ExternalLoginConfirmationViewModel i_Model)
        {
            ApplicationUser userToReturn;
            var externalIdentity = getExternalIdentity();
            var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name").Value;
            var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;
            string gender = getGender(externalIdentity);
            var userID = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:id").Value;
            var birthDate = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:birthdate").Value;
            var imageURL = string.Format(@"https://graph.facebook.com/{0}/picture?type=normal", userID);
            //var emailClaim = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            userToReturn = new ApplicationUser()
            {
                UserName = GenerateUserName(i_Model.Email),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Gender = gender,
                ImageUrl = imageURL
            };
            return userToReturn;
        }

        private string getGender(Task<ClaimsIdentity> externalIdentity)
        {
            string gender = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:gender").Value;
            gender = gender.ToLower();
            if (gender == "female")
            {
                return "F";
            }
            else if (gender == "male")
            { return "M"; }
            else
            { return ""; }
        }

        public string GetGender()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user.Gender == "M")
            { return "Male"; }
            else if (user.Gender == "F")
            { return "Female"; }
            else
            { return ""; }
        }
        private Task<ClaimsIdentity> getExternalIdentity()
        {
            return HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                if (error.Contains("Name") && error.Contains("is already taken."))
                {
                    ModelState.AddModelError("", "Email is allready taken");
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
        }
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
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