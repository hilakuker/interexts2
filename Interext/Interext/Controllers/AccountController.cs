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
using System.Net;
using System.Globalization;
using System.Drawing;
using System.Data.Entity;

namespace Interext.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
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
        public ActionResult ShowProfile(string returnUrl)
        {
            //var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var user = UserManager.FindById(User.Identity.GetUserId());
            ProfileViewModel profile = new ProfileViewModel();
            //profile.UserName = user.UserName;
            profile.FirstName = user.FirstName;
            profile.LastName = user.LastName;
            profile.Email = user.Email;
            if (user.Gender == "F")
                profile.Gender = "Female";
            else
                profile.Gender = "Male";

            profile.ImageUrl = user.ImageUrl;
            profile.BirthDate = getBirthdateAndAge(user.BirthDate.Value);
            profile.Address = user.HomeAddress;
            profile.InterestsToDisplay = InterestsFromObjects.GetInterestsForDisplay(user.Interests.ToList());

            List<Event> attendingEvents = db.EventVsAttendingUsers.Where(e => e.UserId == user.Id).Select(e => e.Event).Where(x => x.DateTimeOfTheEvent >= DateTime.Today).ToList();
            int thisMonth = DateTime.Today.Month;
            List<EventsByMonth> eventsByMonth = new List<EventsByMonth>();

            getMonthEvents(thisMonth, 12, attendingEvents, eventsByMonth);
            getMonthEvents(1, thisMonth - 1, attendingEvents, eventsByMonth);

            List<Event> createdEvents = db.Events.Where(e => e.CreatorUser.Id == user.Id && e.EventStatus == e_EventStatus.Active).OrderByDescending(x => x.DateTimeOfTheEvent).ToList();
            List<EventsByMonth> createdEventsByMonth = new List<EventsByMonth>();

            getMonthEvents(thisMonth, 12, createdEvents, createdEventsByMonth);
            getMonthEvents(1, thisMonth - 1, createdEvents, createdEventsByMonth);


            List<Event> pastEvents = db.EventVsAttendingUsers.Where(e => e.UserId == user.Id).Select(e => e.Event).Where(x => x.DateTimeOfTheEvent < DateTime.Today).OrderByDescending(x => x.DateTimeOfTheEvent).ToList();

            ViewData["EventsByMonth"] = eventsByMonth;
            ViewData["CreatedEventsByMonth"] = createdEventsByMonth;
            ViewData["PastEvents"] = pastEvents;

            //profile.Events = attendingEvents;
            return View(profile);
        }

        private void getMonthEvents(int fromMonthIndex, int toMonthIndex, List<Event> allEvents, List<EventsByMonth> eventsByMonth)
        {
            for (int i = fromMonthIndex; i <= toMonthIndex; i++)
            {
                var monthEvents = allEvents.Where(x => x.DateTimeOfTheEvent.Month == i);
                if (monthEvents.Count() > 0)
                {
                    List<Event> monthEventsList = monthEvents.OrderBy(x => x.DateTimeOfTheEvent).ToList();
                    string monthName = getMonthName(i);
                    eventsByMonth.Add(new EventsByMonth { Month = monthName, MonthEvents = monthEventsList });
                }
            }
        }

        private string getMonthName(int monthIndex)
        {
            string monthName = "";
            switch (monthIndex)
            {
                case 1:
                    monthName = "January";
                    break;
                case 2:
                    monthName = "February";
                    break;
                case 3:
                    monthName = "March";
                    break;
                case 4:
                    monthName = "April";
                    break;
                case 5:
                    monthName = "May";
                    break;
                case 6:
                    monthName = "June";
                    break;
                case 7:
                    monthName = "July";
                    break;
                case 8:
                    monthName = "August";
                    break;
                case 9:
                    monthName = "September";
                    break;
                case 10:
                    monthName = "October";
                    break;
                case 11:
                    monthName = "November";
                    break;
                case 12:
                    monthName = "December";
                    break;
            }

            return monthName;
        }

        

        private string getBirthdateAndAge(DateTime i_BirthDate)
        {
            string finalResult = "";
            // int todayYear = DateTime.Today.Year;
            string birthDateString = i_BirthDate.ToShortDateString();
            finalResult = string.Format("{0} ({1} year old)", birthDateString, caculateAge(i_BirthDate).ToString());
            return finalResult;
        }
        public ActionResult DeleteUser()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var userFromDb = db.Users.Find(user.Id);
            userFromDb.UserName = "";
            userFromDb.AccountIsActive = false;
            List<EventVsAttendingUser> eventVsAttendingUsers = db.EventVsAttendingUsers.Where(x => x.UserId == user.Id).ToList();
            foreach (var eventVsAttendingUser in eventVsAttendingUsers)
            {
                db.EventVsAttendingUsers.Remove(eventVsAttendingUser);
            }
            List<EventVsWaitingApprovalUser> eventVsWaitingApprovalUsers = db.EventVsWaitingApprovalUsers.Where(x => x.UserId == user.Id).ToList();
            foreach (var eventVsWaitingApprovalUser in eventVsWaitingApprovalUsers)
            {
                db.EventVsWaitingApprovalUsers.Remove(eventVsWaitingApprovalUser);
            }
            List<Event> userCreatedEvents = db.Events.Where(x => x.CreatorUser.Id == user.Id).ToList();
            foreach (Event eventCreated in userCreatedEvents)
            {
                eventCreated.EventStatus = e_EventStatus.Deleted;
                db.Entry(eventCreated).State = EntityState.Modified;
                List<EventVsAttendingUser> eventVsAttendingUsers2 = db.EventVsAttendingUsers.Where(x => x.Event.Id == eventCreated.Id).ToList();
                foreach (var eventVsAttendingUser2 in eventVsAttendingUsers2)
                {
                    db.EventVsAttendingUsers.Remove(eventVsAttendingUser2);
                }

                List<EventVsWaitingApprovalUser> eventVsWaitingApprovalUsers2 = db.EventVsWaitingApprovalUsers.Where(x => x.Event.Id == eventCreated.Id).ToList();
                foreach (var eventVsWaitingApprovalUser2 in eventVsWaitingApprovalUsers2)
                {
                    db.EventVsWaitingApprovalUsers.Remove(eventVsWaitingApprovalUser2);
                }

                List<Comment> comments = db.Comments.Where(x => x.Event.Id == eventCreated.Id).ToList();
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                }
            }    

            db.SaveChanges();
            return LogOff();
        }

        public ActionResult Edit()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            EditProfileViewModel profile = new EditProfileViewModel();
            profile.FirstName = user.FirstName;
            profile.LastName = user.LastName;
            if (user.Gender == "F")
                profile.Gender = "Female";
            else
                profile.Gender = "Male";

            profile.ImageUrl = user.ImageUrl;
            profile.BirthDate = user.BirthDate.Value;
            profile.BirthDateDay = user.BirthDate.Value.Day;
            profile.BirthDateMonth = user.BirthDate.Value.Month;
            profile.BirthDateYear = user.BirthDate.Value.Year;
            profile.Address = user.HomeAddress;

            ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(user.Interests, db);

            buildDateViewBags();
            return View(profile);
        }

        private void buildDateViewBags()
        {
            int[] days = new int[31];
            for (int i = 0; i < 31; i++)
            {
                days[i] = i + 1;
            }
            ViewBag.Days = days;

            int[] months = new int[12];
            for (int i = 0; i < 12; i++)
            {
                months[i] = i + 1;
            }
            ViewBag.Months = months;

            int[] years = new int[121];
            for (int i = 0; i <= 120; i++)
            {
                years[i] = DateTime.Today.Year - i;
            }
            
            ViewBag.Years = years;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProfileViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ImageUrl == null)
            {
                model.ImageUrl = user.ImageUrl;
            }
            else
            {
                if (!ImageSaver.IsImage(ImageUrl))
                {
                    ModelState.AddModelError("Image Upload", "You can upload only images");
                }
            }
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser updatedUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                updatedUser.FirstName = model.FirstName;
                updatedUser.LastName = model.LastName;
                updatedUser.Gender = model.Gender;
                updatedUser.BirthDate = model.BirthDate.Date;
                updatedUser.HomeAddress = model.Address;
                updatedUser.Interests.Clear();
                updatedUser.Interests = GetSelectedInterests(selectedInterests);
                uploadAndSetImage(ref updatedUser, ImageUrl);
                db.SaveChanges();
                ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(updatedUser.Interests, db);
                return Redirect("/Account/ShowProfile");
            }
            ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(user.Interests, db);
            buildDateViewBags();
            return View(model);
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
                else if (!user.AccountIsActive)
                {
                    ViewBag.Message = "Would you like to reactivate your account?";
                    return View();
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
        public string GetImageUrl()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.ImageUrl;
            }
            return "";
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(db);
            buildDateViewBags();
            RegisterViewModel model = new RegisterViewModel();
            model.BirthDate = new DateTime(DateTime.Now.Year - 18, 1, 1);
            model.BirthDateDay = 1;
            model.BirthDateMonth = 1;
            model.BirthDateYear = DateTime.Now.Year - 18;
            return View(model);
        }

        private List<Interest> GetSelectedInterests(string selectedInterests)
        {
            List<Interest> interests = new List<Interest>();
            foreach (string item in selectedInterests.Split(','))
            {
                if (item != "")
                {
                    int id;
                    if (int.TryParse(item, out id))
                    {
                        Interest interest = db.Interests.SingleOrDefault(x => x.Id == id);
                        interests.Add(interest);
                    }
                }
            }
            return interests;
        }




        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ImageUrl == null)
            {
                ModelState.AddModelError("Image Upload", "Image Upload is required");
            }
            else
            {
                if (!ImageSaver.IsImage(ImageUrl))
                {
                    ModelState.AddModelError("Image Upload", "You can upload only images");
                }
            }
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = GenerateUserName(model.Email),
                    Email = model.Email,
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.FirstName),
                    LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.LastName),
                    Gender = model.Gender,
                    BirthDate = model.BirthDate.Date,
                    HomeAddress = model.Address,
                    Age = caculateAge(model.BirthDate),
                    AccountIsActive = true
                };
                List<Interest> interests = GetSelectedInterests(selectedInterests);
                uploadAndSetImage(ref user, ImageUrl);
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationUser newUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                    newUser.Interests = interests;
                    db.SaveChanges();

                    await SignInAsync(user, isPersistent: false);
                    ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(newUser.Interests, db);
                    return Redirect("/Account/RegisterApproval");
                }
                else
                {
                    ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(interests, db);
                    AddErrors(result);
                }
            }
            else
            {
                if (selectedInterests != "")
                {
                    List<Interest> interests = GetSelectedInterests(selectedInterests);
                    ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(interests, db);
                }
                else
                {
                    ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(db);
                }
            }
            buildDateViewBags();
            return View(model);
        }

        public ActionResult RegisterApproval()
        {
            return View();
        }

        private void uploadAndSetImage(ref ApplicationUser user, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                user.ImageUrl = ImageSaver.SaveUser(user.Id, ImageUrl, Server);
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
            if (user != null && user.AccountIsActive)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else if (user.AccountIsActive == false)
            {
                return RedirectToAction("Login");
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                Task<ClaimsIdentity> externalIdentity = getExternalIdentity();
                var email = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var firstName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:first_name").Value;
                var lastName = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:last_name").Value;
                string gender = getGender(externalIdentity);
                var userID = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:id").Value;
                var imageURL = string.Format(@"https://graph.facebook.com/{0}/picture?height=160&type=normal&width=160", userID);
                var birthdayObj = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth);
                DateTime birthday;
                if (birthdayObj != null)
                {
                    if (!DateTime.TryParse(birthdayObj.Value, out birthday))
                    {
                        birthday = new DateTime(DateTime.Now.Year - 10, 1, 1);
                    }
                }
                else
                {
                    birthday = new DateTime(DateTime.Now.Year - 10, 1, 1);
                }
                ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(db);
                buildDateViewBags();
                return View("ExternalLoginConfirmation",
                    new ExternalLoginConfirmationViewModel
                    {
                        Email = email,
                        FirstName = firstName,
                        Gender = gender,
                        ImageUrl = imageURL,
                        LastName = lastName,
                        BirthDate = birthday,
                        BirthDateDay = birthday.Day,
                        BirthDateMonth = birthday.Month,
                        BirthDateYear = birthday.Year
                    });
            }
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
        public async Task<ActionResult> ExternalLoginConfirmation
            (ExternalLoginConfirmationViewModel model, HttpPostedFileBase ImageUrl,
            string returnUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            // Get the information about the user from the external login provider
            var info = await AuthenticationManager_GetExternalLoginInfoAsync_Workaround();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user;
                user = createUserFromFacebookInfo(model, ImageUrl);
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        ApplicationUser newUser = db.Users.SingleOrDefault(x => x.UserName == user.UserName);
                        newUser.Interests = GetSelectedInterests(selectedInterests);
                        db.SaveChanges();
                        await SignInAsync(user, isPersistent: false);
                        ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(newUser.Interests, db);
                        return Redirect("/Account/RegisterApproval");
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                if (selectedInterests != "")
                {
                    List<Interest> interests = GetSelectedInterests(selectedInterests);
                    ViewBag.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(interests, db);
                }
                else
                {
                    ViewBag.AllInterests = InterestsFromObjects.InitAllInterests(db);
                }

                var externalIdentity = getExternalIdentity();
                var userID = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:id").Value;
                model.ImageUrl = string.Format(@"https://graph.facebook.com/{0}/picture?height=160&type=normal&width=160", userID);
                buildDateViewBags();
            }
            return View(model);
        }

        private ApplicationUser createUserFromFacebookInfo(ExternalLoginConfirmationViewModel i_Model, HttpPostedFileBase ImageUrl)
        {
            ApplicationUser userToReturn;
            var externalIdentity = getExternalIdentity();
            var userID = externalIdentity.Result.Claims.FirstOrDefault(c => c.Type == "urn:facebook:id").Value;

            var birthdate = i_Model.BirthDate;
            userToReturn = new ApplicationUser()
            {
                UserName = GenerateUserName(i_Model.Email),
                FirstName = i_Model.FirstName,
                LastName = i_Model.LastName,
                Email = i_Model.Email,
                Gender = i_Model.Gender,
                BirthDate = birthdate,
                Age = caculateAge(birthdate),
                HomeAddress = i_Model.Address,
                AccountIsActive = true
            };
            if (ImageUrl == null)
            {
                userToReturn.ImageUrl = string.Format(@"https://graph.facebook.com/{0}/picture?height=160&type=normal&width=160", userID);
            }
            else
            {
                uploadAndSetImage(ref userToReturn, ImageUrl);
            }
            return userToReturn;
        }

        private int caculateAge(DateTime birthdate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;
            if (birthdate > today.AddYears(-age))
            { age--; }
            return age;
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

        public ActionResult Details(string id)
        {
            ViewData["Id"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser @user = db.Users.Find(id);
            if (@user == null)
            {
                return HttpNotFound();
            }
            ProfileViewModel profileToShow = new ProfileViewModel()
            {
                Address = @user.HomeAddress,
                BirthDate = getBirthdateAndAge(user.BirthDate.Value),
                Email = @user.Email,
                Interests = @user.Interests,
                //Events = @user.Events,
                FirstName = @user.FirstName,
                Gender = @user.Gender,
                LastName = @user.LastName,
                ImageUrl = @user.ImageUrl,
                UserName = @user.UserName,
                InterestsToDisplay = InterestsFromObjects.GetInterestsForDisplay(user.Interests.ToList())
            };

            List<Event> attendingEvents = db.EventVsAttendingUsers.Where(e => e.UserId == user.Id).Select(e => e.Event).Where(x=>x.DateTimeOfTheEvent >= DateTime.Today).ToList();
            int thisMonth = DateTime.Today.Month;
            List<EventsByMonth> eventsByMonth = new List<EventsByMonth>();

            getMonthEvents(thisMonth, 12, attendingEvents, eventsByMonth);
            getMonthEvents(1, thisMonth - 1, attendingEvents, eventsByMonth);

            List<Event> createdEvents = db.Events.Where(e => e.CreatorUser.Id == user.Id && e.EventStatus == e_EventStatus.Active).OrderByDescending(x => x.DateTimeOfTheEvent).ToList();
            List<EventsByMonth> createdEventsByMonth = new List<EventsByMonth>();

            getMonthEvents(thisMonth, 12, createdEvents, createdEventsByMonth);
            getMonthEvents(1, thisMonth - 1, createdEvents, createdEventsByMonth);


            List<Event> pastEvents = db.EventVsAttendingUsers.Where(e => e.UserId == user.Id).Select(e => e.Event).Where(x => x.DateTimeOfTheEvent < DateTime.Today).OrderByDescending(x=>x.DateTimeOfTheEvent).ToList();

            ViewData["EventsByMonth"] = eventsByMonth;
            ViewData["CreatedEventsByMonth"] = createdEventsByMonth;
            ViewData["PastEvents"] = pastEvents;
            return View(profileToShow);
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