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

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
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
                InterestsToDisplay = GetInterestsForDisplay(@event.Interests.ToList())
            };
            return View(eventToShow);
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

        // GET: /Event/Create
        public ActionResult Create()
        {
            EventViewModel model = new EventViewModel();
            //model.Title = "dd";
            model.AllInterests = InitAllInterests();
            //ViewBag.AllInterests = InitAllInterests();
            return View(model);
            //return View();
        }
        private List<InterestViewModel> InitAllInterests()
        {
            List<InterestViewModel> allInterests = new List<InterestViewModel>();
            List<Interest> categories = db.Interests.Where(x => x.InterestsCategory == null).ToList();
            foreach (var item in categories)
            {
                InterestViewModel category = new InterestViewModel { Id = item.Id, ImageUrl = item.ImageUrl, Title = item.Title, SubInterests = new List<InterestViewModel>(), IsSelected = false };
                foreach (var subitem in db.Interests.Where(x => x.InterestsCategory.Id == category.Id))
                {
                    InterestViewModel subcategory = new InterestViewModel { Id = subitem.Id, ImageUrl = subitem.ImageUrl, Title = subitem.Title, SubInterests = null, IsSelected = false };
                    category.SubInterests.Add(subcategory);
                }
                allInterests.Add(category);
            }
            return allInterests;
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

            var allSubCategories = interests.Where(x => x.InterestsCategory != null);
            foreach (var item in allSubCategories)
            {
                var categoryId = item.InterestsCategory.Id;
                if (interests.SingleOrDefault(x => x.Id == categoryId) == null)
                {
                    //the category is not in the list, so check if all its subcategories are in the list
                    //if so, add it to the list
                    var allSubCategoriesOfTheCategory = db.Interests.Where(x => x.InterestsCategory.Id == categoryId);
                    var allSubCategoriesOfTheCategoriesChecked = interests.Where(x => x.InterestsCategory.Id == categoryId);
                    if (allSubCategoriesOfTheCategory.Count() == allSubCategoriesOfTheCategoriesChecked.Count())
                    {
                        //all the sub categories are checked, but the category itself is not. Add it to the list
                        Interest category = db.Interests.SingleOrDefault(x => x.Id == categoryId);
                        interests.Add(category);
                    }
                }
            }

            return interests;
        }

        private List<InterestViewModel> LoadAllInterestsWithSelected(Event e)
        {
            List<InterestViewModel> allInterests = InitAllInterests();
            foreach (var item in e.Interests)
            {
                InterestViewModel interest = allInterests.SingleOrDefault(x => x.Id == item.Id);
                if (interest != null)
                    interest.IsSelected = true;
                else
                {
                    foreach (var item2 in allInterests)
                    {
                        InterestViewModel interest2 = item2.SubInterests.SingleOrDefault(x => x.Id == item.Id);
                        if (interest2 != null)
                            interest2.IsSelected = true;
                    }
                }
            }
            return allInterests;
        }

        // POST: /Event/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
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
                    Interests = GetSelectedInterests(selectedInterests)
                };
                db.Events.Add(eventToCreate);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var a = ex.EntityValidationErrors;
                }
                saveImage(ref eventToCreate, ImageUrl);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = eventToCreate.Id });
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
                return string.Format("<a href=\"/Event/Edit?id="+ model.Id +"\">Edit event<a/>");
            }
            else
            {
                return "";
            }
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
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
                DateOfTheEvent = @event.DateTimeOfTheEvent,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                BackroundColor = @event.BackroundColor,
                BackroundColorOpacity = @event.BackroundColorOpacity,
                DateOfTheEvent = @event.DateTimeOfTheEvent.Date,
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
                AllInterests = LoadAllInterestsWithSelected(@event),
                InterestsToDisplay = GetInterestsForDisplay(@event.Interests.ToList())
            };
            // setAllInterests(@event, model);
            setSideOfText(@event, model);
            setGenderOptions(@event, model);
            return View(model);
        }


        // POST: /Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventViewModel model, HttpPostedFileBase ImageUrl, string selectedInterests)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(User.Identity.GetUserId()); 
                Event @event = db.Events.Find(model.Id);
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
                @event.Interests = GetSelectedInterests(selectedInterests);

                setSideOfText(@event, model);
                setGenderOptions(@event, model);
                db.Entry(@event).State = EntityState.Modified;

                //model.AllInterests = LoadAllInterestsWithSelected(@event);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                }
                return RedirectToAction("Details", new {id = @event.Id});
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

        private void setGenderOptions(Event @event, ref EventViewModel model)
        {
            model.GenderParticipantOptions = new Dictionary<string, bool>();
            model.GenderParticipantOptions.Add("Female", String.Equals("Female", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Male", String.Equals("Male", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Both", String.Equals("Both", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
        }

        //private void setAllInterests(Event @event, EventViewModel model)
        //{
        //    model.AllInterests = new List<InterestViewModel>();
        //    List<Interest> categories = db.Interests.Where(x => x.InterestsCategory == null).ToList();
        //    foreach (var item in categories)
        //    {
        //        InterestViewModel category = new InterestViewModel { Id = item.Id, ImageUrl = item.ImageUrl, Title = item.Title, SubInterests = new List<InterestViewModel>(), IsSelected = false };
        //        foreach (var subitem in db.Interests.Where(x => x.InterestsCategory.Id == category.Id))
        //        {
        //            InterestViewModel subcategory = new InterestViewModel { Id = subitem.Id, ImageUrl = subitem.ImageUrl, Title = subitem.Title, SubInterests = null, IsSelected = true };
        //            category.SubInterests.Add(subcategory);
        //        }
        //        model.AllInterests.Add(category);
        //    }
        //}



        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            CascadingDeleteHelper.Delete(@event, null);
            db.Events.Remove(@event);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
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
