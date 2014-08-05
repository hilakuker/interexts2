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
        //private InterextDB db = new InterextDB();

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Event/

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
                Place = @event.Place,
                Title = @event.Title,
                Description = @event.Description,
            };
            return View(eventToShow);
        }

        private string setDisplayDateFormat(DateTime dateTime)
        {
            string date = dateTime.Date.ToString();
            string[] dateSplit = date.Split('/');
            string minute = getHour(dateTime.Minute);
            string hour = getHour(dateTime.Hour);
            string rightDateFormat = string.Format("{0}.{1} {2}:{3}", dateSplit[0], dateSplit[1], 
                hour, minute);
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


        private void setSideOfText(Event @event, ref EventViewModel eventToShow)
        {
            eventToShow.SideOfTextOptions = new Dictionary<string, bool>();
            eventToShow.SideOfTextOptions.Add("Right", String.Equals("Right",@event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Left", String.Equals("Left",@event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Top", String.Equals("Top", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
            eventToShow.SideOfTextOptions.Add("Bottom", String.Equals("Bottom", @event.SideOfText, StringComparison.OrdinalIgnoreCase));
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            //var model = new EventViewModel();
            //model.AllInterests = db.Interests.ToList();
            //return View(model);

            //ViewBag.AllInterests = db.Interests.ToList();
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventViewModel model, HttpPostedFileBase ImageUrl)
        {
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
                    PlaceLongitude = model.PlaceLongitude
                };
                db.Events.Add(eventToCreate);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                   var a= ex.EntityValidationErrors;
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
        //public ActionResult Create([Bind(Include = "Id,Title,Description,ImageUrl,DateTimeOfTheEvent")] Event @event, HttpPostedFileBase ImageUrl, string[] Interests)
        ////{
        ////public ActionResult Create(HttpPostedFileBase ImageUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Event dbEvent = new Event();
        //        ImageSaver.SaveImage(ImageUrl, Server.MapPath("~/App_Data/uploads/events"), ImageUrl.FileName);
        //        @event.DateTimeCreated = DateTime.Now;
        //        //dbEvent.DateTimeOfTheEvent = @event.DateTimeOfTheEvent;
        //        //dbEvent.Description = @event.Description;
        //        //dbEvent.ImageUrl = @event.ImageUrl;



        //        @event.Interests = new List<Interest>();




        //        //dbEvent.Place = @event.Place;
        //        //dbEvent.Title = @event.Title;
        //        //dbEvent.UsersAttending = @event.UsersAttending;
        //        //dbEvent.UsersInvited = @event.UsersInvited;

        //        foreach (string interestID in Interests)
        //        {
        //            int id = int.Parse(interestID);
        //            Interest interest = db.Interests.SingleOrDefault(x => x.Id == id);
        //            if (interest != null)
        //            {
        //                @event.Interests.Add(interest);
        //            }
        //        }

        //        db.Events.Add(@event);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(@event);
        //}

        // GET: /Event/Edit/5

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
            EventViewModel model = new EventViewModel()
            {
                Id = @event.Id,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                BackroundColor = @event.BackroundColor,
                BackroundColorOpacity = @event.BackroundColorOpacity,
                DateOfTheEvent = @event.DateTimeOfTheEvent.Date,
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
                Title = @event.Title
            };
            setSideOfText(@event, ref model);
            setGenderOptions(@event, ref model);
            return View(model);
        }


        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       //[Route("Event/edit?id={id:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventViewModel model, HttpPostedFileBase ImageUrl)
        {
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
                @event.BackroundColor = model.BackroundColor.Replace("rgb(","");
                @event.BackroundColor = @event.BackroundColor.Replace(")", "");
                @event.DateTimeOfTheEvent = model.DateTimeOfTheEvent;
                @event.Description = model.Description;
                @event.GenderParticipant = model.GenderParticipant;
                db.Entry(@event).State = EntityState.Modified;
                @event.PlaceLatitude = model.PlaceLatitude;
                @event.PlaceLongitude = model.PlaceLongitude;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                }
                setSideOfText(@event, ref model);
                return RedirectToAction("Details", new {id = @event.Id});
            }
            return View(model);
        }

        private void setGenderOptions(Event @event, ref EventViewModel model)
        {
            model.GenderParticipantOptions = new Dictionary<string, bool>();
            model.GenderParticipantOptions.Add("Female", String.Equals("Female", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Male", String.Equals("Male", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
            model.GenderParticipantOptions.Add("Both", String.Equals("Both", @event.GenderParticipant, StringComparison.OrdinalIgnoreCase));
        }
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
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                    UserName = string.Format ("{0} {1}",user.FirstName, user.LastName), 
                    Checked = false});
            }
            return View(userList);
        }

    }
}
