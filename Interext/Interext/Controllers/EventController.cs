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

namespace Interext.Controllers
{
    public class EventController : Controller
    {
        //private InterextDB db = new InterextDB();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Event/
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
            return View(@event);
        }

        // GET: /Event/Create
        public ActionResult Create()
        {
            //var model = new EventViewModel();
            //model.AllInterests = db.Interests.ToList();
            //return View(model);

            ViewBag.AllInterests = db.Interests.ToList();
            return View();
        }

        // POST: /Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ImageUrl,DateTimeOfTheEvent")] Event @event, HttpPostedFileBase ImageUrl, string[] Interests)
        //{
        //public ActionResult Create(HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                //Event dbEvent = new Event();
                ImageSaver.SaveImage(ImageUrl, Server.MapPath("~/App_Data/uploads/events"), ImageUrl.FileName);
                //if (ImageUrl != null)
                //{
                //    if (ImageUrl.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(ImageUrl.FileName);
                //       // var uri = "~/App_Data/uploads/events" + @event.Id;
                //        // need to create folder for each event, the name of the folder is the id of the event
                //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads/events"), fileName);
                //        ImageUrl.SaveAs(path);
                //        @event.ImageUrl = fileName;
                //    }
                //}
                @event.DateTimeCreated = DateTime.Now;
                //dbEvent.DateTimeOfTheEvent = @event.DateTimeOfTheEvent;
                //dbEvent.Description = @event.Description;
                //dbEvent.ImageUrl = @event.ImageUrl;



                @event.Interests = new List<Interest>();




                //dbEvent.Place = @event.Place;
                //dbEvent.Title = @event.Title;
                //dbEvent.UsersAttending = @event.UsersAttending;
                //dbEvent.UsersInvited = @event.UsersInvited;

                foreach (string interestID in Interests)
                {
                    int id = int.Parse(interestID);
                    Interest interest = db.Interests.SingleOrDefault(x => x.Id == id);
                    if (interest != null)
                    {
                        @event.Interests.Add(interest);
                    }
                }

                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

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
    }
}
