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
            if (@event == null)
            {
                return HttpNotFound();
            }
            if (@event.EventStatus == e_EventStatus.Deleted)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventViewModel eventToShow = new EventViewModel()
            {
                CreatorUser = @event.CreatorUser,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
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
                TimeSet = @event.TimeSet,
                AgeOfParticipantsSet = @event.AgeOfParticipantsSet,
                InterestsToDisplay = InterestsFromObjects.GetInterestsForDisplay(@event.Interests.ToList())
            };
            List<ApplicationUser> attendingUsers = db.EventVsAttendingUsers.Where
                (e => e.EventId == eventToShow.Id).Select(e => e.AttendingUser).Where(e => e.AccountIsActive).ToList();
            eventToShow.UsersAttending = attendingUsers;

            List<ApplicationUser> usersWaitingApproval = db.EventVsWaitingApprovalUsers.Where
                (e => e.EventId == eventToShow.Id).Select(e => e.WaitingApprovalUser).Where(e => e.AccountIsActive).ToList();
            eventToShow.UsersWaitingApproval = usersWaitingApproval;



            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                eventToShow.Comments = SetDeleteButtonsForComments(@event.Comments.ToList(), (ApplicationUser)user, eventToShow.CreatorUser);
                if (user.Id == eventToShow.CreatorUser.Id)
                {
                    eventToShow.CurrentUserIsCreator = true;
                    ViewData["UserAlreadyAttending"] = false;
                }
                else if (User.IsInRole("Admin"))
                { eventToShow.CurrentUserIsCreator = true; }
                else
                {
                    eventToShow.CurrentUserIsCreator = false;
                    if (eventToShow.UsersAttending.SingleOrDefault(x => x.Id == user.Id) != null)
                    {
                        ViewData["UserAlreadyAttending"] = true;
                    }
                    if (eventToShow.UsersWaitingApproval.SingleOrDefault(x => x.Id == user.Id) != null)
                    {
                        ViewData["UserAlreadyAttending"] = true;
                    }
                }
            }
            else
            {
                eventToShow.CurrentUserIsCreator = false;
                eventToShow.Comments = SetDeleteButtonsForComments(@event.Comments.ToList(), null, eventToShow.CreatorUser);
                ViewData["UserAlreadyAttending"] = false;
            }
            Statistics Statistics = LoadStatistics(attendingUsers);
            var json = new JavaScriptSerializer().Serialize(Statistics);
            ViewData["Statistics"] = json;
            return View(eventToShow);
        }
        private List<Comment> SetDeleteButtonsForComments(List<Comment> eventComments, ApplicationUser thisUser, ApplicationUser eventCreator)
        {
            List<Comment> comments = new List<Comment>();
            foreach (Comment item in eventComments)
            {
                if (thisUser == null)
                {
                    item.ShowDeleteButton = false;
                }
                else
                {
                    if (thisUser.Id == item.Author.Id || thisUser.Id == eventCreator.Id || User.IsInRole("Admin"))
                    {
                        item.ShowDeleteButton = true;
                    }
                    else
                    {
                        item.ShowDeleteButton = false;
                    }
                }
                comments.Add(item);
            }
            return comments;
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


            foreach (ApplicationUser user in attenders)
            {
                if (user.Interests.Count > 0)
                {
                    List<Interest> mainCategoriesOfTheSubCategories = new List<Interest>();
                    foreach (Interest subCategory in user.Interests.Where(x => x.InterestsCategory != null))
                    {
                        Interest category = db.Interests.SingleOrDefault(x => x.Id == subCategory.InterestsCategory.Id);
                        mainCategoriesOfTheSubCategories.Add(category);
                    }
                    mainCategoriesOfTheSubCategories = mainCategoriesOfTheSubCategories.Distinct().ToList();
                    foreach (Interest item in mainCategoriesOfTheSubCategories)
                    {
                        StatisticItem statisticItem = interestsStatistics.SingleOrDefault(x => x.categoryId == item.Id);
                        if (statisticItem == null)
                        {
                            //insert the category for the first time
                            interestsStatistics.Add(new StatisticItem { categoryId = item.Id, number = 1, title = item.Title });
                        }
                        else
                        {
                            statisticItem.number++;
                        }
                    }
                }
            }

            interestsStatistics = interestsStatistics.OrderByDescending(x => x.number).Take(5).ToList();
            return interestsStatistics;
        }

        private List<SubCategoriesStatistics> LoadSubInterestsStatistics(List<ApplicationUser> attenders, List<StatisticItem> mainCategoriesStatistics)
        {
            List<SubCategoriesStatistics> subCategoriesStatisticsList = new List<SubCategoriesStatistics>();

            List<Interest> allSubInterests = new List<Interest>();
            int numberOfUsers = attenders.Count();
            foreach (ApplicationUser user in attenders)
            {
                if (user.Interests.Count > 0)
                {
                    allSubInterests.AddRange(user.Interests.Where(x => x.InterestsCategory != null));
                }
            }

            foreach (StatisticItem mainCategory in mainCategoriesStatistics)
            {
                SubCategoriesStatistics subCategoriesStatistics = new SubCategoriesStatistics { categoryId = mainCategory.categoryId, categoryTitle = mainCategory.title, subCategories = new List<StatisticItem>() };

                var biggestSubCategories = allSubInterests.Where(x => x.InterestsCategory.Id == mainCategory.categoryId).GroupBy(x => new { x.Id, x.Title }).Select(x => new { x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(3);
                foreach (var item in biggestSubCategories)
                {
                    StatisticItem s = new StatisticItem();
                    s.categoryId = item.Key.Id;
                    s.title = item.Key.Title;
                    float persentage = (float)item.Count / (float)numberOfUsers;
                    s.number = (int)(persentage * 100);
                    subCategoriesStatistics.subCategories.Add(s);
                }
                subCategoriesStatisticsList.Add(subCategoriesStatistics);
            }
            return subCategoriesStatisticsList;
        }
        //private List<StatisticItem> LoadInterestsStatistics(List<ApplicationUser> attenders)
        //{
        //    List<StatisticItem> interestsStatistics = new List<StatisticItem>();

        //    List<Interest> allInterests = new List<Interest>();
        //    foreach (ApplicationUser user in attenders)
        //    {
        //        if (user.Interests.Count > 0)
        //        {
        //            allInterests.AddRange(user.Interests);
        //        }
        //    }
        //    var d1 = allInterests.Distinct();
        //    List<Interest> uniqueInterests = d1.ToList();
        //    foreach (Interest item in uniqueInterests)
        //    {
        //        if (item.InterestsCategory == null)
        //        {
        //            if (db.Interests.Where(x => x.InterestsCategory.Id == item.Id).Count() == 0)
        //            {
        //                int count = allInterests.Where(x => x.Id == item.Id).Count();
        //                interestsStatistics.Add(new StatisticItem { number = count, title = item.Title });
        //            }
        //        }
        //        else
        //        {

        //            int count = allInterests.Where(x => x.Id == item.Id).Count();
        //            interestsStatistics.Add(new StatisticItem { number = count, title = item.Title });

        //            //Interest category = item.InterestsCategory;
        //            //if (interestsStatistics.Where(x => x.categoryId == category.Id).Count() == 0)
        //            //{
        //            //    int count = allInterests.Where(x => ((x.InterestsCategory != null) ? x.InterestsCategory.Id == category.Id : true)).Count();
        //            //    interestsStatistics.Add(new StatisticItem { number = count, title = category.Title, categoryId = category.Id });
        //            //}

        //        }
        //    }
        //    interestsStatistics = interestsStatistics.OrderByDescending(x => x.number).Take(5).ToList();
        //    return interestsStatistics;
        //}


        private Statistics LoadStatistics(List<ApplicationUser> attenders)
        {
            Statistics statistics = new Statistics();
            statistics.Gender = LoadGenderStatistics(attenders);
            statistics.Age = LoadAgeStatistics(attenders);
            statistics.Interests = LoadInterestsStatistics(attenders);
            statistics.SubCategoriesInterests = LoadSubInterestsStatistics(attenders, statistics.Interests);

            return statistics;
        }


        private string setDisplayDateFormat(DateTime dateTime)
        {
            string date = dateTime.Date.ToString();
            string[] dateSplit = date.Split('/');
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

        private void setSideOfText(string sideOfText, EventViewModel eventToShow)
        {
            eventToShow.SideOfTextOptions = new Dictionary<string, bool>();
            eventToShow.SideOfTextOptions.Add("Right", String.Equals("Right", sideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Left", String.Equals("Left", sideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Top", String.Equals("Top", sideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Bottom", String.Equals("Bottom", sideOfText, StringComparison.OrdinalIgnoreCase));
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
            //add the categories if all the subcategories are selected 


            return interests;
        }
        private string defaultDraftImage = "/Content/images/example-images/draft-image.jpg";

        [Authorize]
        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            model.AllInterests = InterestsFromObjects.InitAllInterests(db);
            ViewBag.DraftImage = defaultDraftImage;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            List<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            if (model.isImageFromStock)
            {
                if (String.IsNullOrEmpty(model.ImageFromStock))
                {
                    ModelState.AddModelError("Image Upload", "Image Upload is required");
                }
            }
            else
            {
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
            }
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }
            if (model.TimeSet == true && model.DateTimeOfTheEvent < DateTime.Now)
            {
                ModelState.AddModelError("Event Date time", "Event date cannot occur in the past.");
            }
            if (model.TimeSet == false && model.DateTimeOfTheEvent.Date < DateTime.Today)
            {
                ModelState.AddModelError("Event Date", "Event date cannot occur in the past.");
            }

            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                Event eventToCreate = new Event()
                {
                    CreatorUser = user,
                    AgeOfParticipantsMax = model.AgeOfParticipantsMax,
                    AgeOfParticipantsMin = model.AgeOfParticipantsMin,
                    AgeOfParticipantsSet = true,
                    GenderParticipant = model.GenderParticipant,
                    BackroundColor = model.BackroundColor,
                    BackroundColorOpacity = model.BackroundColorOpacity,
                    DateTimeCreated = DateTime.Now,
                    Place = model.Place,
                    Title = model.Title,
                    Description = model.Description,
                    SideOfText = model.SideOfText,
                    DateTimeOfTheEvent = model.DateTimeOfTheEvent,
                    TimeSet = model.TimeSet,
                    PlaceLatitude = model.PlaceLatitude,
                    PlaceLongitude = model.PlaceLongitude,
                    Interests = GetSelectedInterests(selectedInterests),
                    EventStatus = e_EventStatus.Active,
                    isImageFromStock = model.isImageFromStock,
                    PrivacyType = model.PrivacyType
                };
                if (model.AgeOfParticipantsMax == null && model.AgeOfParticipantsMin == null)
                {
                    eventToCreate.AgeOfParticipantsSet = false;
                }
                if (model.isImageFromStock)
                {
                    eventToCreate.ImageUrl = model.ImageFromStock;
                }
                else
                {
                    eventToCreate.ImageUrl = model.ImageUrl;
                }

                db.Events.Add(eventToCreate);
                try
                {
                    db.SaveChanges();
                    if (!model.isImageFromStock)
                    {
                        saveImage(ref eventToCreate, ImageUrl);
                    }
                    EventVsAttendingUser eventVsAttendingUser = new EventVsAttendingUser { EventId = eventToCreate.Id, UserId = user.Id, Event = eventToCreate, AttendingUser = user };
                    db.EventVsAttendingUsers.Add(eventVsAttendingUser);

                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = eventToCreate.Id });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult error in ex.EntityValidationErrors)
                    {
                        ModelState.AddModelError("", error.ValidationErrors.ToString());
                    }
                }
            }
            else
            {
                if (selectedInterests != "")
                {
                    List<Interest> interests = GetSelectedInterests(selectedInterests);
                    model.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(interests, db);
                }
                else
                {
                    model.AllInterests = InterestsFromObjects.InitAllInterests(db);
                }
                if (model.isImageFromStock)
                {
                    ViewBag.DraftImage = model.ImageFromStock;
                }
                else
                {
                    ViewBag.DraftImage = defaultDraftImage;
                }
            }
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
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                Event @event = db.Events.Find(id);
                if (@event == null)
                {
                    return false;
                }
                if (@event.PrivacyType == e_PrivacyType.Public)
                {
                    EventVsAttendingUser eventVsAttendingUser = new EventVsAttendingUser { EventId = @event.Id, UserId = user.Id, Event = @event, AttendingUser = user };
                    db.EventVsAttendingUsers.Add(eventVsAttendingUser);
                }
                else
                {
                    EventVsWaitingApprovalUser eventVsWaitingApprovalUser = new EventVsWaitingApprovalUser { EventId = @event.Id, UserId = user.Id, Event = @event, WaitingApprovalUser = user };
                    db.EventVsWaitingApprovalUsers.Add(eventVsWaitingApprovalUser);
                }
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
            return UnjoinUserFromEvent(User.Identity.GetUserId(), id);
        }

        public bool AcceptUser(string UserId, int? EventId)
        {
            try
            {
                if (UserId == "" || EventId == null)
                {
                    return false;
                }

                EventVsWaitingApprovalUser eventVsWaitingApprovalUser = db.EventVsWaitingApprovalUsers.SingleOrDefault(x => x.Event.Id == EventId && x.WaitingApprovalUser.Id == UserId);
                if (eventVsWaitingApprovalUser != null)
                {
                    Event @event = db.Events.SingleOrDefault(x => x.Id == EventId);
                    if (@event == null)
                    {
                        return false;
                    }
                    ApplicationUser user = db.Users.SingleOrDefault(x => x.Id == UserId);
                    if (user == null)
                    {
                        return false;
                    }

                    EventVsAttendingUser eventVsAttendingUser = new EventVsAttendingUser { EventId = @event.Id, UserId = user.Id, Event = @event, AttendingUser = user };
                    db.EventVsAttendingUsers.Add(eventVsAttendingUser);

                    db.EventVsWaitingApprovalUsers.Remove(eventVsWaitingApprovalUser);
                    db.SaveChanges();

                }


                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UnjoinUserFromEvent(string UserId, int? EventId)
        {
            try
            {
                if (EventId == null)
                {
                    return false;
                }
                var user = UserManager.FindById(UserId);
                Event @event = db.Events.Find(EventId);
                if (@event == null)
                {
                    return false;
                }

                EventVsWaitingApprovalUser eventVsWaitingApprovalUser = db.EventVsWaitingApprovalUsers.SingleOrDefault(x => x.Event.Id == @event.Id && x.WaitingApprovalUser.Id == user.Id);
                if (eventVsWaitingApprovalUser != null)
                {
                    db.EventVsWaitingApprovalUsers.Remove(eventVsWaitingApprovalUser);
                    db.SaveChanges();
                }

                EventVsAttendingUser eventVsAttendingUser = db.EventVsAttendingUsers.SingleOrDefault(x => x.Event.Id == @event.Id && x.AttendingUser.Id == user.Id);
                if (eventVsAttendingUser != null)
                {
                    db.EventVsAttendingUsers.Remove(eventVsAttendingUser);
                    db.SaveChanges();
                }


                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
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
            string hour = @event.DateTimeOfTheEvent.ToString("HH");
            string minute = @event.DateTimeOfTheEvent.ToString("mm");
            string dateString = @event.DateTimeOfTheEvent.Date.ToString();
            EventViewModel model = new EventViewModel()
            {
                Id = @event.Id,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                AgeOfParticipantsSet = @event.AgeOfParticipantsSet,
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
                Place = @event.Place,
                SideOfText = @event.Place,
                Title = @event.Title,
                PlaceLongitude = @event.PlaceLongitude,
                PlaceLatitude = @event.PlaceLatitude,
                TimeSet = @event.TimeSet,
                AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(@event.Interests, db),
                InterestsToDisplay = InterestsFromObjects.GetInterestsForDisplay(@event.Interests.ToList()),
                PrivacyType = @event.PrivacyType
            };
            if (@event.isImageFromStock)
            {
                model.ImageFromStock = @event.ImageUrl;
                model.isImageFromStock = true;
            }
            List<ApplicationUser> attendingUsers = db.EventVsAttendingUsers.Where
                 (e => e.EventId == @event.Id).Select(e => e.AttendingUser).Where(e => e.AccountIsActive).ToList();
            model.UsersAttending = attendingUsers;

            setSideOfText(@event.SideOfText, model);
            setGenderOptions(@event.GenderParticipant, model);
            setPrivacyTypeOptions((e_PrivacyType)@event.PrivacyType, model);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            List<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            Event @event = db.Events.Find(model.Id);
            if (selectedInterests == "")
            {
                ModelState.AddModelError("Interests select", "You need to select interests");
            }
            if (model.TimeSet == true && model.DateTimeOfTheEvent < DateTime.Now)
            {
                ModelState.AddModelError("Event Date time", "Event date cannot occur in the past.");
            }
            if (model.TimeSet == false && model.DateTimeOfTheEvent.Date < DateTime.Today)
            {
                ModelState.AddModelError("Event Date", "Event date cannot occur in the past.");
            }
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                @event.CreatorUser = user;
                @event.Title = model.Title;
                @event.SideOfText = model.SideOfText;
                @event.Place = model.Place;
                @event.isImageFromStock = model.isImageFromStock;
                if (model.ImageUrl != null)
                {
                    saveImage(ref @event, ImageUrl);
                }
                else
                {
                    if (@event.isImageFromStock)
                    {
                        @event.ImageUrl = model.ImageFromStock;
                    }
                }
                @event.AgeOfParticipantsMax = model.AgeOfParticipantsMax;
                @event.AgeOfParticipantsMin = model.AgeOfParticipantsMin;
                @event.BackroundColor = model.BackroundColor.Replace("rgb(", "");
                @event.BackroundColor = @event.BackroundColor.Replace(")", "");
                @event.BackroundColorOpacity = model.BackroundColorOpacity;
                @event.DateTimeOfTheEvent = model.DateTimeOfTheEvent;
                @event.Description = model.Description;
                @event.GenderParticipant = model.GenderParticipant;
                @event.PlaceLatitude = model.PlaceLatitude;
                @event.PlaceLongitude = model.PlaceLongitude;
                @event.Interests.Clear();
                @event.Interests = InterestsFromObjects.GetSelectedInterests(selectedInterests, db);
                @event.AgeOfParticipantsSet = true;
                @event.PrivacyType = model.PrivacyType;
                if (model.AgeOfParticipantsMax == null && model.AgeOfParticipantsMin == null)
                {
                    @event.AgeOfParticipantsSet = false;
                }
                @event.TimeSet = model.TimeSet;
                setSideOfText(@event.SideOfText, model);
                setGenderOptions(@event.GenderParticipant, model);
                setPrivacyTypeOptions((e_PrivacyType)@event.PrivacyType, model);
                db.Entry(@event).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = @event.Id });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult error in ex.EntityValidationErrors)
                    {
                        ModelState.AddModelError("", error.ValidationErrors.ToString());
                    }
                }

            }
            else
            {
                List<ApplicationUser> attendingUsers = db.EventVsAttendingUsers.Where
                 (e => e.EventId == @event.Id).Select(e => e.AttendingUser).Where(e => e.AccountIsActive).ToList();
                model.UsersAttending = attendingUsers;
                setSideOfText(model.SideOfText, model);
                setGenderOptions(model.GenderParticipant, model);
                setPrivacyTypeOptions((e_PrivacyType)model.PrivacyType, model);
                if (selectedInterests != "")
                {
                    List<Interest> interests = GetSelectedInterests(selectedInterests);
                    model.AllInterests = InterestsFromObjects.LoadInterestViewModelsFromInterests(interests, db);
                }
                else
                {
                    model.AllInterests = InterestsFromObjects.InitAllInterests(db);
                }
                model.InterestsToDisplay = selectedInterests;
                model.CreatorUser = @event.CreatorUser;
                if (model.isImageFromStock)
                {
                    model.ImageUrl = model.ImageFromStock;
                }
                else
                {
                    model.ImageUrl = @event.ImageUrl;
                }
                model.BackroundColor = model.BackroundColor.Replace("rgb(", "");
                model.BackroundColor = model.BackroundColor.Replace(")", "");
            }


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

        private void setGenderOptions(string GenderParticipant, EventViewModel model)
        {
            model.GenderParticipantOptions = new Dictionary<string, bool>();
            model.GenderParticipantOptions.Add("Both", String.Equals("Both", GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Male", String.Equals("Male", GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Female", String.Equals("Female", GenderParticipant, StringComparison.OrdinalIgnoreCase));
        }
        private void setGenderOptions(EventViewModel model)
        {
            setGenderOptions(model.GenderParticipant, model);
        }

        private void setPrivacyTypeOptions(e_PrivacyType PrivacyType, EventViewModel model)
        {
            model.PrivacyTypeOptions = new Dictionary<e_PrivacyType, bool>();
            model.PrivacyTypeOptions.Add(e_PrivacyType.Public, PrivacyType == e_PrivacyType.Public);
            model.PrivacyTypeOptions.Add(e_PrivacyType.RequiresApproval, PrivacyType == e_PrivacyType.RequiresApproval);
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
                List<EventVsAttendingUser> eventVsAttendingUsers = db.EventVsAttendingUsers.Where(x => x.Event.Id == @event.Id).ToList();
                foreach (var eventVsAttendingUser in eventVsAttendingUsers)
                {
                    db.EventVsAttendingUsers.Remove(eventVsAttendingUser);
                }

                List<EventVsWaitingApprovalUser> eventVsWaitingApprovalUsers = db.EventVsWaitingApprovalUsers.Where(x => x.Event.Id == @event.Id).ToList();
                foreach (var eventVsWaitingApprovalUser in eventVsWaitingApprovalUsers)
                {
                    db.EventVsWaitingApprovalUsers.Remove(eventVsWaitingApprovalUser);
                }

                List<Comment> comments = db.Comments.Where(x => x.Event.Id == @event.Id).ToList();
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveComment(string comment, string Id)
        {
            List<Comment> model = null;

            if (String.IsNullOrEmpty(comment))
            {
                ModelState.AddModelError("Empty Comment", "Please fill the comment field");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    int eventId = int.Parse(Id);
                    Event currentEvent = db.Events.SingleOrDefault(x => x.Id == eventId);
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    Comment commentItem = new Comment();
                    commentItem.Author = user;
                    commentItem.DateTimeCreated = DateTime.Now;
                    commentItem.Event = currentEvent;
                    commentItem.Text = comment;
                    db.Comments.Add(commentItem);
                    db.SaveChanges();
                    model = db.Comments.Where(x => x.Event.Id == eventId).ToList();
                    model = SetDeleteButtonsForComments(model, (ApplicationUser)user, currentEvent.CreatorUser);
                    ViewData["Id"] = eventId;
                    return PartialView("~/Views/Event/_Comments.cshtml", model);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Saving failed", "Error. Saving the comment failed");
                }
            }
            return View(model);
        }

        public ActionResult DeleteComment(int? id, int EventId)
        {
            List<Comment> model = null;
            try
            {
                if (id != null)
                {
                    Comment comment = db.Comments.SingleOrDefault(x => x.Id == id);
                    if (comment != null)
                    {
                        db.Comments.Remove(comment);
                        db.SaveChanges();
                        model = db.Comments.Where(x => x.Event.Id == EventId).ToList();
                        Event currentEvent = db.Events.SingleOrDefault(x => x.Id == EventId);
                        var user = UserManager.FindById(User.Identity.GetUserId());
                        model = SetDeleteButtonsForComments(model.ToList(), (ApplicationUser)user, currentEvent.CreatorUser);

                        ViewData["Id"] = EventId;
                        return PartialView("~/Views/Event/_Comments.cshtml", model);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Delete failed", "Error. Comment delete failed");
            }
            return View(model);
        }
    }
}
