using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interext.Models;
using System.IO;
using Interext.OtherCalsses;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Text;
using System.Web.Script.Serialization;

namespace Interext.Controllers
{
    public class EventController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private UserManager<ApplicationUser> UserManager { get; set; }
        public EventController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        public ActionResult Index()
        {
            List<Event> f = db.Events.ToList();
            return View(db.Events.ToList());
        }

        public ActionResult Details(int? id)
        {
            ViewData["Id"] = id;
            ViewData["UserAlreadyAttending"] = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event.EventStatus == e_EventStatus.Deleted)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (@event == null)
            {
                return HttpNotFound();
            }

            EventViewModel eventToShow = new EventViewModel()
            {
                CreatorUser = @event.CreatorUser,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                NumOfParticipantsMax = @event.NumOfParticipantsMax,
                NumOfParticipantsMin = @event.NumOfParticipantsMin,
                ImageUrl = @event.ImageUrl,
                GenderParticipant = @event.GenderParticipant,
                BackroundColor = @event.BackroundColor,
                BackroundColorOpacity = @event.BackroundColorOpacity,
                DateOfTheEventNoYear = setDisplayDateFormat(@event.DateTimeOfTheEvent),
                DateTimeOfTheEvent = @event.DateTimeOfTheEvent,
                Place = @event.Place,
                Title = @event.Title,
                Description = @event.Description,
                Id = @event.Id,
                InterestsToDisplay = GetInterestsForDisplay(@event.Interests.ToList()),
                UsersAttending = @event.UsersAttending
            };
            if (user.Id == eventToShow.CreatorUser.Id)
            {
                eventToShow.CurrentUserIsCreator = true;
                ViewData["UserAlreadyAttending"] = false;
            }
            else
            {
                eventToShow.CurrentUserIsCreator = false;
                if (eventToShow.UsersAttending.SingleOrDefault(x => x.Id == user.Id) != null)
                {
                    ViewData["UserAlreadyAttending"] = true;
                }
            }
            Statistics Statistics = LoadStatistics(eventToShow.UsersAttending.ToList());
            var json = new JavaScriptSerializer().Serialize(Statistics);
            ViewData["Statistics"] = json;
            return View(eventToShow);
        }
        private List<StatisticItem> LoadGenderStatistics(List<ApplicationUser> attenders)
        {
            List<StatisticItem> genderStatistics = new List<StatisticItem>();

            List<string> genders = attenders.Select(x => x.Gender).ToList();
            int females = genders.Where(x => x == "F").Count();
            int males = genders.Where(x => x == "M").Count();

            genderStatistics.Add(new StatisticItem { number = females, title = "Female" });
            genderStatistics.Add(new StatisticItem { number = males, title = "Male" });
            return genderStatistics;
        }

        private List<StatisticItem> LoadAgeStatistics(List<ApplicationUser> attenders)
        {
            List<StatisticItem> ageStatistics = new List<StatisticItem>();
            int[] agesCount = new int[24]; // 0-5, 6-10, 11-15, 16-20, 21-25, 26-30...
            for (int i = 0; i < agesCount.Length; i++)
            {
                agesCount[i] = 0;
            }

            List<int> ages = attenders.Select(x => x.Age).ToList();
            foreach (int age in ages)
            {
                int j = (age - 1) / 5;   // 15/5 = 3 16 /5 = 3, 20/5  = 4
                agesCount[j]++;
            }
            for (int j = 0; j < agesCount.Length; j++)
            {
                int ageCount = agesCount[j];
                if (ageCount != 0)
                {
                    int ageFrom = j * 5 + 1;
                    string ageTitle = ageFrom.ToString() + "-" + (ageFrom + 4).ToString();
                    ageStatistics.Add(new StatisticItem { number = ageCount, title = ageTitle });
                }
            }
            return ageStatistics;
        }

        private List<StatisticItem> LoadInterestsStatistics(List<ApplicationUser> attenders)
        {
            List<StatisticItem> interestsStatistics = new List<StatisticItem>();

            List<Interest> allInterests = new List<Interest>();
            foreach (ApplicationUser user in attenders)
            {
                if (user.Interests.Count > 0)
                {
                    allInterests.AddRange(user.Interests);
                }
            }
            var d1 = allInterests.Distinct();
            List<Interest> uniqueInterests = d1.ToList();
            foreach (Interest item in uniqueInterests)
            {
                if (item.InterestsCategory == null)
                {
                    if (db.Interests.Where(x => x.InterestsCategory.Id == item.Id).Count() == 0)
                    {
                        int count = allInterests.Where(x => x.Id == item.Id).Count();
                        interestsStatistics.Add(new StatisticItem { number = count, title = item.Title });
                    }
                }
                else
                {
                    int count = allInterests.Where(x => x.Id == item.Id).Count();
                    interestsStatistics.Add(new StatisticItem { number = count, title = item.Title });
                }
            }
            interestsStatistics = interestsStatistics.OrderByDescending(x => x.number).Take(10).ToList();
            return interestsStatistics;
        }

        private Statistics LoadStatistics(List<ApplicationUser> attenders)
        {
            Statistics statistics = new Statistics();
            statistics.Gender = LoadGenderStatistics(attenders);
            statistics.Age = LoadAgeStatistics(attenders);
            statistics.Interests = LoadInterestsStatistics(attenders);

            return statistics;
        }
        private string GetInterestsForDisplay(List<Interest> Interests)
        {
            string interestsForDisplay = "";
            foreach (var interest in Interests)
            {
                //add sub categories only if their category is not in the interests list
                if (Interests.Where(x => x.Id == interest.InterestsCategory.Id) == null || interest.InterestsCategory == null)
                {
                    interestsForDisplay += interest.Title + ", ";
                }
            }
            if (interestsForDisplay != "")
            {
                interestsForDisplay = interestsForDisplay.Remove(interestsForDisplay.Count() - 2, 2);
            }
            return interestsForDisplay;
        }

        private string setDisplayDateFormat(DateTime dateTime)
        {
            string date = dateTime.Date.ToString();
            string[] dateSplit = date.Split('/');
            //string minute = getHour(dateTime.Minute);
            //string hour = getHour(dateTime.Hour);
            //string rightDateFormat = string.Format("{0}.{1} {2}:{3}", dateSplit[0], dateSplit[1], 
            //    hour, minute);
            string rightDateFormat = string.Format("{0}.{1}", dateSplit[0], dateSplit[1]);
            return rightDateFormat;
        }

        private string getHour(int number)
        {
            if (number < 10)
            {
                return string.Format("0{0}", number);
            }
            return number.ToString();
        }

        private void setSideOfText(Event @event, EventViewModel eventToShow)
        {
            eventToShow.SideOfTextOptions = new Dictionary<string, bool>();
            eventToShow.SideOfTextOptions.Add("Right", String.Equals("Right", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Left", String.Equals("Left", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Top", String.Equals("Top", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Bottom", String.Equals("Bottom", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
        }

        private void setSideOfText(EventViewModel eventToShow)
        {
            eventToShow.SideOfTextOptions = new Dictionary<string, bool>();
            eventToShow.SideOfTextOptions.Add("Right", String.Equals("Right", eventToShow.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Left", String.Equals("Left", eventToShow.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Top", String.Equals("Top", eventToShow.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Bottom", String.Equals("Bottom", eventToShow.SideOfText, StringComparison.OrdinalIgnoreCase));
        }

        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            //model.Title = "dd";
            model.AllInterests = InterestsFromObjects.InitAllInterests(db);
            //ViewBag.AllInterests = InitAllInterests();
            return View(model);
            //return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ImageUrl == null)
            {
                ModelState.AddModelError("Image Upload", "Image Upload is required");
            }
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            } 
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                Event eventToCreate = new Event()
                {
                    CreatorUser = user,
                    AgeOfParticipantsMax = model.AgeOfParticipantsMax,
                    AgeOfParticipantsMin = model.AgeOfParticipantsMin,
                    NumOfParticipantsMax = model.NumOfParticipantsMax,
                    NumOfParticipantsMin = model.NumOfParticipantsMin,
                    ImageUrl = model.ImageUrl,
                    GenderParticipant = model.GenderParticipant,
                    BackroundColor = model.BackroundColor,
                    BackroundColorOpacity = model.BackroundColorOpacity,
                    DateTimeCreated = DateTime.Now,
                    Place = model.Place,
                    Title = model.Title,
                    Description = model.Description,
                    SideOfText = model.SideOfText,
                    DateTimeOfTheEvent = model.DateTimeOfTheEvent,
                    PlaceLatitude = model.PlaceLatitude,
                    PlaceLongitude = model.PlaceLongitude,
                    Interests = InterestsFromObjects.GetSelectedInterests(selectedInterests, db),
                    EventStatus = e_EventStatus.Active,
                    UsersAttending = new List<ApplicationUser>()
                };
                eventToCreate.UsersAttending.Add(user);
                db.Events.Add(eventToCreate);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult error in ex.EntityValidationErrors)
                    {
                        ModelState.AddModelError("", error.ValidationErrors.ToString());
                    }
                }
                saveImage(ref eventToCreate, ImageUrl);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = eventToCreate.Id });
            }
            InterestsFromObjects.LoadAllInterestsFromEventView(selectedInterests, model, db);
            return View(model);
        }

        private void saveImage(ref Event eventToCreate, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                eventToCreate.ImageUrl = ImageSaver.SaveEvent(eventToCreate.Id, ImageUrl, Server);
            }
        }

        public string CheckIfUserCanEditEvent(EventViewModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id == model.CreatorUser.Id)
            {
                return string.Format("<a href=\"/Event/Edit?id=" + model.Id + "\">Edit event<a/>");
            }
            else
            {
                return "";
            }
        }
        public string CheckIfUserCanJoinEvent(EventViewModel model)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id == model.CreatorUser.Id)
            {
                return string.Format("<a href=\"/Event/JoinEvent?id=" + model.Id + "\">Join event<a/>");
            }
            else
            {
                return "";
            }
        }


        public bool JoinEvent(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }
                var user = UserManager.FindById(User.Identity.GetUserId());
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return false;
                }

                @event.UsersAttending.Add(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UnjoinEvent(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }
                var user = UserManager.FindById(User.Identity.GetUserId());
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return false;
                }
                ApplicationUser userAttending = @event.UsersAttending.SingleOrDefault(x => x.Id == user.Id);
                if (userAttending != null)
                {
                    //ViewBag["UserAlreadyAttending"] = false;
                    @event.UsersAttending.Remove(userAttending);
                    db.SaveChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != @event.CreatorUser.Id)
            {
                return RedirectToAction("Index", "Home");
            }
            if (@event == null)
            {
                return HttpNotFound();
            }
            string hour = @event.DateTimeOfTheEvent.Hour.ToString();
            string minute = @event.DateTimeOfTheEvent.Minute.ToString();
            string dateString = @event.DateTimeOfTheEvent.Date.ToString();
            EventViewModel model = new EventViewModel()
            {
                Id = @event.Id,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                BackroundColor = @event.BackroundColor,
                BackroundColorOpacity = @event.BackroundColorOpacity,
                DateOfTheEvent = @event.DateTimeOfTheEvent.ToShortDateString(),
                DateOfTheEventNoYear = string.Format("{0}.{1}", 
                @event.DateTimeOfTheEvent.Day.ToString(), @event.DateTimeOfTheEvent.Month.ToString()),
                DateTimeOfTheEvent = @event.DateTimeOfTheEvent,
                HourTimeOfTheEvent = hour,
                MinuteTimeOfTheEvent = minute,
                CreatorUser = @event.CreatorUser,
                DateTimeCreated = @event.DateTimeCreated,
                Description = @event.Description,
                GenderParticipant = @event.GenderParticipant,
                ImageUrl = @event.ImageUrl,
                NumOfParticipantsMax = @event.NumOfParticipantsMax,
                NumOfParticipantsMin = @event.NumOfParticipantsMin,
                Place = @event.Place,
                SideOfText = @event.Place,
                Title = @event.Title,
                PlaceLongitude = @event.PlaceLongitude,
                PlaceLatitude = @event.PlaceLatitude,
                AllInterests = InterestsFromObjects.LoadAllInterestsFromEvent(@event, db),
                InterestsToDisplay = GetInterestsForDisplay(@event.Interests.ToList())
            };
            // setAllInterests(@event, model);
            setSideOfText(@event, model);
            setGenderOptions(@event, model);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }
            Event @event = db.Events.Find(model.Id);
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                @event.CreatorUser = user;
                @event.Title = model.Title;
                @event.SideOfText = model.SideOfText;
                @event.Place = model.Place;
                if (model.ImageUrl != null)
                {
                    saveImage(ref @event, ImageUrl);
                }
                @event.NumOfParticipantsMax = model.NumOfParticipantsMax;
                @event.NumOfParticipantsMin = model.NumOfParticipantsMin;
                @event.AgeOfParticipantsMax = model.AgeOfParticipantsMax;
                @event.AgeOfParticipantsMin = model.AgeOfParticipantsMin;
                @event.BackroundColor = model.BackroundColor.Replace("rgb(", "");
                @event.BackroundColor = @event.BackroundColor.Replace(")", "");
                @event.DateTimeOfTheEvent = model.DateTimeOfTheEvent;
                @event.Description = model.Description;
                @event.GenderParticipant = model.GenderParticipant;
                @event.PlaceLatitude = model.PlaceLatitude;
                @event.PlaceLongitude = model.PlaceLongitude;
                @event.Interests = InterestsFromObjects.GetSelectedInterests(selectedInterests, db);

                setSideOfText(@event, model);
                setGenderOptions(@event, model);
                db.Entry(@event).State = EntityState.Modified;

                model.AllInterests = InterestsFromObjects.LoadAllInterestsFromEvent(@event, db);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                }
                return RedirectToAction("Details", new { id = @event.Id });
            }
            InterestsFromObjects.LoadAllInterestsFromEventView(selectedInterests, model, db);
            setSideOfText(model);
            setGenderOptions(model);
            model.InterestsToDisplay = selectedInterests;
            model.CreatorUser = @event.CreatorUser;
            return View(model);
        }

        private void setSideOfTextDefault(ref EventViewModel model)
        {
            model.SideOfTextOptions = new Dictionary<string, bool>();
            model.SideOfTextOptions.Add("Right", String.Equals("Right", model.SideOfText, StringComparison.OrdinalIgnoreCase));
            model.SideOfTextOptions.Add("Left", String.Equals("Left", model.SideOfText, StringComparison.OrdinalIgnoreCase));
            model.SideOfTextOptions.Add("Top", String.Equals("Top", model.SideOfText, StringComparison.OrdinalIgnoreCase));
            model.SideOfTextOptions.Add("Bottom", String.Equals("Bottom", model.SideOfText, StringComparison.OrdinalIgnoreCase));
        }

        private void setGenderOptions(Event @event, EventViewModel model)
        {
            model.GenderParticipantOptions = new Dictionary<string, bool>();
            model.GenderParticipantOptions.Add("Female", String.Equals("Female", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Male", String.Equals("Male", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Both", String.Equals("Both", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
        }
        private void setGenderOptions(EventViewModel model)
        {
            model.GenderParticipantOptions = new Dictionary<string, bool>();
            model.GenderParticipantOptions.Add("Female", String.Equals("Female", model.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Male", String.Equals("Male", model.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Both", String.Equals("Both", model.GenderParticipant, StringComparison.OrdinalIgnoreCase));
        }
        public bool Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return false;
                }
                @event.EventStatus = e_EventStatus.Deleted;
                @event.CreatorUser = @event.CreatorUser;
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return true;

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GetUsers()
        {
            List<ChooseUsersViewModel> userList = new List<ChooseUsersViewModel>();
            foreach (ApplicationUser user in db.Users.ToList())
            {
                userList.Add(new ChooseUsersViewModel()
                {
                    ImageUrl = user.ImageUrl,
                    UserName = string.Format("{0} {1}", user.FirstName, user.LastName),
                    Checked = false
                });
            }
            return View(userList);
        }

    }
}
