using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interext.Models;

namespace Interext.Controllers
{
    public class InterestController : Controller
    {
        //private InterextDB db = new InterextDB();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Interest/
        public ActionResult Index()
        {
            return View(db.Interests.ToList());
        }

        // GET: /Interest/
        public ActionResult ChooseInterests(int id, string fieldName)
        {

            ViewBag.Interests = db.Interests.ToList();
            ViewBag.FieldName = fieldName;

            return View("_ChooseInterests");
        }


        // GET: /Interest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // GET: /Interest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Interest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Interest interest)
        {
            if (ModelState.IsValid)
            {
                db.Interests.Add(interest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interest);
        }

        // GET: /Interest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // POST: /Interest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Interest interest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interest);
        }

        // GET: /Interest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interest interest = db.Interests.Find(id);
            if (interest == null)
            {
                return HttpNotFound();
            }
            return View(interest);
        }

        // POST: /Interest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interest interest = db.Interests.Find(id);
            db.Interests.Remove(interest);
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
