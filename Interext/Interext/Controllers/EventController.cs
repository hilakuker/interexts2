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
                CreatorUserId = @event.CreatorUserId,
                AgeOfParticipantsMax = @event.AgeOfParticipantsMax,
                AgeOfParticipantsMin = @event.AgeOfParticipantsMin,
                NumOfParticipantsMax = @event.NumOfParticipantsMax,
                NumOfParticipantsMin = @event.NumOfParticipantsMin,
                ImageUrl = @event.ImageUrl,
                GenderParticipant = @event.GenderParticipant,
                BackroundColor = @event.BackroundColor,
                BackroundColorOpacity = @event.BackroundColorOpacity,
                DateTimeCreated = @event.DateTimeCreated,
                Place = @event.Place,
                Title = @event.Title,
                Description = @event.Description,
                SideOfText = @event.SideOfText,
                DateTimeOfTheEvent = @event.DateTimeOfTheEvent
            };

            return View(eventToShow);
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
                    CreatorUserId = user.Id,
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
                    DateTimeOfTheEvent = model.DateTimeOfTheEvent
                };
                db.Events.Add(eventToCreate);
                db.SaveChanges();
                saveImage(ref eventToCreate, ImageUrl);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
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
            return View(@event);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,ImageUrl,DateTimeOfTheEvent,DateTimeCreated")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
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
